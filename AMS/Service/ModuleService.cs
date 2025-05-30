using AMS.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AMS.Service
{
    public class ModuleService
    {
        private readonly IConfiguration _config;

        public ModuleService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<List<ModuleDTO>> GetModulesForUserAsync(string userName, bool isAdmin)
        {
            var modules = new List<ModuleDTO>();
            var connectionString = _config.GetConnectionString("DefaultConnection");

            using (var conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                if (isAdmin)
                {
                    // if admin then he access all module 
                    cmd = new SqlCommand("SELECT Id, Name, Url FROM Modules", conn);
                }
                else
                {
                    
                    cmd = new SqlCommand("GetModulesForUserRoles", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);
                }

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        modules.Add(new ModuleDTO
                        {
                            ModuleName = reader["Name"].ToString(),
                            ModuleUrl = reader["Url"] == DBNull.Value ? "#" : reader["Url"].ToString()
                        });
                    }
                }
            }

            return modules;
        }
    }
}
