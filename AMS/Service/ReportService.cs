using AMS.Model;
using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AMS.Service
{
    public class ReportService
    {
        private readonly IConfiguration _config;

        public ReportService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<List<VoucherViewDTO>> GetVoucherListAsync()
        {
            var list = new List<VoucherViewDTO>();

            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            using (SqlCommand cmd = new SqlCommand("sp_GetVoucherList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new VoucherViewDTO
                        {
                            VoucherID = reader.GetInt32(0),
                            VoucherDate = reader.GetDateTime(1),
                            ReferenceNo = reader.GetString(2),
                            VoucherType = reader.GetString(3),
                            CreatedBy = reader.GetInt32(4),
                            CreatedDate = reader.GetDateTime(5),
                            LineID = reader.GetInt32(6),
                            AccountID = reader.GetInt32(7),
                            AccountName = reader.GetString(8),
                            AccountType = reader.GetString(9),
                            DebitAmount = reader.GetDecimal(10),
                            CreditAmount = reader.GetDecimal(11),
                            Narration = reader.IsDBNull(12) ? null : reader.GetString(12)
                        });
                    }
                }
            }

            return list;
        }

        public MemoryStream ExportToExcelFromList(List<VoucherViewDTO> data)
        {
            var dt = new DataTable("Vouchers");
            dt.Columns.AddRange(new[]
            {
                new DataColumn("Voucher ID"),
                new DataColumn("Date"),
                new DataColumn("Reference No"),
                new DataColumn("Type"),
                new DataColumn("Account Name"),
                new DataColumn("Debit"),
                new DataColumn("Credit"),
                new DataColumn("Narration")
            });

            foreach (var item in data)
            {
                dt.Rows.Add(item.VoucherID, item.VoucherDate.ToShortDateString(), item.ReferenceNo,
                            item.VoucherType, item.AccountName, item.DebitAmount, item.CreditAmount, item.Narration);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(dt, "Voucher Report");

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }

    }
}
