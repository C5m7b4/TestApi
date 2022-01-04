using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TestApi.models;

namespace TestApi.Controllers
{
    [EnableCors("AnyOrigin")]
    [Route("api/users")]
    public class UserController : Controller
    {
        private IConfiguration _config { get; set; }
        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [Route("list")]
        public ActionResult GetUserList(string apikey)
        {
            try
            {
                if (!DbUtil.IsApiKeyValid(apikey, _config))
                {
                    return Ok(new
                    {
                        error = 2,
                        success = false,
                        msg = "Unathorized"
                    });
                }
                DataTable dt = new DataTable();
                List<User> users = new List<User>();

                using( SqlConnection conn = new SqlConnection(DbUtil.GetConnectionString(_config)))
                {
                    string query = "select * from users";
                    conn.Open();
                    using (SqlDataAdapter adapt = new SqlDataAdapter(query, conn))
                    {
                        adapt.Fill(dt);
                        foreach( DataRow dr in dt.Rows)
                        {
                            User u = new User
                            {
                                Id = dr["id"].ToString(),
                                Username = dr["username"].ToString(),
                                FirstName = dr["firstname"].ToString(),
                                LastName = dr["lastname"].ToString(),
                                Email = dr["email"].ToString()
                            };
                            users.Add(u);
                        }
                    }
                    conn.Close();

                }

                return Ok(new
                {
                    error = 0,
                    success = true,
                    users
                });
            }
            catch ( Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    success = false,
                    msg = ex.Message
                });
            }
        }
    }
}
