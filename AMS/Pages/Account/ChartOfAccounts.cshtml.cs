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

        public ChartOfAccountsModel(ChartOfAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task OnGetAsync()
        {
            AllAccounts = await _accountService.GetAllAccountsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(NewAccount.AccountName))
            {
                AllAccounts = await _accountService.GetAllAccountsAsync(); 
                return Page();
            }

            await _accountService.ManageAccountAsync("Insert", NewAccount);

            TempData["Success"] = "Account created successfully!";
            return RedirectToPage(); //for clear
        }
    }

}
