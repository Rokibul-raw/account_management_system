using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AMS.Pages.Module
{
    [Authorize(Roles = "Admin")]
    public class AddModuleModel : PageModel
    {
        private readonly IConfiguration _config;

        public AddModuleModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string ModuleName { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public void OnGet()
        {
            Roles = new List<SelectListItem>
        {
            new("Admin", "Admin"),
            new("Accountant", "Accountant"),
            new("Viewer", "Viewer")
        };
        }
    
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(ModuleName) || string.IsNullOrEmpty(SelectedRole))
                return Page();

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (var command = new SqlCommand("AddModuleAndAssignToRole", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ModuleName", ModuleName);
                    command.Parameters.AddWithValue("@RoleName", SelectedRole);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }

            TempData["Success"] = "Module added and assigned to role successfully!";
            return RedirectToPage("AddModule");
        }
    }
}
