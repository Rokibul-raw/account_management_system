using AMS.Model;
using AMS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMS.Pages.Account
{
    [Authorize(Roles = "Admin,Accountant")]
    public class VoucherModel : PageModel
    {
        private readonly ChartOfAccountService _accountService;

        public VoucherModel(ChartOfAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public VoucherHeaderDTO VoucherHeader { get; set; }

        [BindProperty]
        public List<VoucherLineDTO> VoucherLines { get; set; } = new();

        public List<ChartOfAccount> AccountList { get; set; }

        public async Task OnGetAsync()
        {
            AccountList = await _accountService.GetAllAccountsAsync();

          
            VoucherLines.Add(new VoucherLineDTO());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || !VoucherLines.Any())
            {
                AccountList = await _accountService.GetAllAccountsAsync();
                return Page();
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int user = Convert.ToInt32(userId);
            await _accountService.SaveVoucherAsync(VoucherHeader, VoucherLines, user);

            TempData["Success"] = "Voucher saved successfully!";
            return RedirectToPage();
        }
    }
}
