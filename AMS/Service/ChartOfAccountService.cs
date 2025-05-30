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
    }

}
