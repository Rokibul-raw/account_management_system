using AMS.Model;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMS.Pages.Report
{
    public class ReportModel : PageModel
    {
        private readonly ReportService _reportService;

        public ReportModel(ReportService reportService)
        {
            _reportService = reportService;
        }

        public List<VoucherViewDTO> Vouchers { get; set; }

        public async Task OnGetAsync()
        {
            Vouchers = await _reportService.GetVoucherListAsync();
        }


        public async Task<IActionResult> OnPostExport()
        {
           
            var exportData =await _reportService.GetVoucherListAsync(); 

            var stream = _reportService.ExportToExcelFromList(exportData);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "VoucherReport.xlsx"
            );
        }
    }
}
