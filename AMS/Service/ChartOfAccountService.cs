using AMS.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AMS.Service
{
    public class ChartOfAccountService
    {
        private readonly string _connectionString;

        public ChartOfAccountService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task ManageAccountAsync(string action, ChartOfAccount account)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@Id", (object?)account.Id ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AccountName", account.AccountName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ParentId", (object?)account.ParentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AccountType", account.AccountType ?? (object)DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<ChartOfAccount>> GetAllAccountsAsync()
        {
            var result = new List<ChartOfAccount>();

            using var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand("SELECT * FROM ChartOfAccounts", conn);

            await conn.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            var flatList = new List<ChartOfAccount>();

            while (await reader.ReadAsync())
            {
                flatList.Add(new ChartOfAccount
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    AccountName = reader["AccountName"].ToString(),
                    ParentId = reader["ParentId"] as int?,
                    AccountType = reader["AccountType"].ToString()
                });
            }

            // Build tree
            var lookup = flatList.ToLookup(x => x.ParentId);
            foreach (var item in flatList)
                item.Children = lookup[item.Id].ToList();

            return lookup[null].ToList(); // Top-level accounts
        }



        public async Task<ChartOfAccount> GetAccountByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand("SELECT * FROM ChartOfAccounts WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await conn.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            ChartOfAccount account = null;
            if (await reader.ReadAsync())
            {
                account = new ChartOfAccount
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    AccountName = reader["AccountName"].ToString(),
                    AccountType = reader["AccountType"].ToString(),
                    ParentId = reader["ParentId"] as int?
                };
            }

            return account;
        }


        public async Task SaveVoucherAsync(VoucherHeaderDTO header, List<VoucherLineDTO> lines,int userID)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_SaveVoucher", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@VoucherDate", header.VoucherDate);
            command.Parameters.AddWithValue("@ReferenceNo", header.ReferenceNo ?? "");
            command.Parameters.AddWithValue("@VoucherType", header.VoucherType);
            command.Parameters.AddWithValue("@CreatedBy", userID);

            var table = new DataTable();
            table.Columns.Add("AccountID", typeof(int));
            table.Columns.Add("DebitAmount", typeof(decimal));
            table.Columns.Add("CreditAmount", typeof(decimal));
            table.Columns.Add("Narration", typeof(string));

            foreach (var item in lines)
                table.Rows.Add(item.AccountID, item.DebitAmount, item.CreditAmount, item.Narration);

            var tvpParam = command.Parameters.AddWithValue("@VoucherLines", table);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "VoucherLineType";

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

}
