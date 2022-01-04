using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi
{
    
    public static class DbUtil
    {
        private static readonly string _encrypt = "!!@8TestApi8@!!";

        public static readonly string apikey = "234lh643kjh245435";

        public static string GetConnectionString(IConfiguration _config)
        {
            var server = Encryption.Decrypt(_config["DB:Server"], _encrypt);
            var database = Encryption.Decrypt(_config["DB:Database"], _encrypt);
            var username = Encryption.Decrypt(_config["DB:Username"], _encrypt);
            var password = Encryption.Decrypt(_config["DB:Password"], _encrypt);
            var connectionString = $"Server={server}; Initial Catalog={database}; User Id={username}; password={password}";
            return connectionString;

        }

        public static bool IsApiKeyValid(string apikey, IConfiguration config)
        {
            try
            {
                using ( SqlConnection conn = new SqlConnection(GetConnectionString(config)))
                {
                    string query = "select count(*) from apikeys where apikey='" + apikey + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    string result = cmd.ExecuteScalar().ToString();
                    conn.Close();
                    int iResult = 0;
                    int.TryParse(result, out iResult);
                    if ( iResult > 0 )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch( Exception ex)
            {
                string err = ex.Message;
                return false;
            }
        }
    }
}
