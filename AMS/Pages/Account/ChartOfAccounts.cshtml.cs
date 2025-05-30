using AMS.Model;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
    
namespace AMS.Pages.Account
{
    public class ChartOfAccountsModel : PageModel
    {
        private readonly ChartOfAccountService _accountService;

        [BindProperty]
        public ChartOfAccount NewAccount { get; set; }

        public List<ChartOfAccount> AllAccounts { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        [BindProperty]
        public ChartOfAccount AccountForm { get; set; }

     

        public ChartOfAccountsModel(ChartOfAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task OnGetAsync()
        {
            AllAccounts = await _accountService.GetAllAccountsAsync();
          
            if (Id.HasValue)
            {
                AccountForm = await _accountService.GetAccountByIdAsync(Id.Value);
                
               
            }
            else
            {
                AccountForm = new ChartOfAccount();  
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(AccountForm.AccountName))
            {
                AllAccounts = await _accountService.GetAllAccountsAsync(); 
                return Page();
            }

            if (AccountForm.Id > 0)
            {
                 //update
                await _accountService.ManageAccountAsync("Update", AccountForm);
                TempData["Success"] = "Account updated successfully!";
            }
            else
            {
                await _accountService.ManageAccountAsync("Insert", AccountForm);
                TempData["Success"] = "Account created successfully!";
            }


            TempData["Success"] = "Account created successfully!";
            return RedirectToPage(); //for clear
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (id > 0)
            {
                await _accountService.ManageAccountAsync("Delete", new ChartOfAccount { Id = id });
                TempData["Success"] = "Account deleted successfully!";
            }

            return RedirectToPage();
        }
    }

}
