using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

namespace JaCoolMVC.Controllers
{
    public class FranchiseModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string contact_num { get; set; }
        public string location { get; set; }
        public string size { get; set; }
        public string status { get; set; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                // Clear session and sign out
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                return Json(new { success = true, redirectUrl = Url.Action("Login", "Home") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        public ActionResult Admin()
        {
            var franchiseData = GetFranchiseDataFromDatabase();

            return View(franchiseData);
        }

        private List<FranchiseModel> GetFranchiseDataFromDatabase()
        {
            var connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\pc\Documents\Visual Studio 2012\Projects\JaCoolMVC\JaCoolMVC\App_Data\JACOOL_DB.mdf;Integrated Security=True";
            var franchiseData = new List<FranchiseModel>();

            using (var cn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string query = "SELECT ID, NAME, EMAIL, CONTACT_NUM, LOCATION, SIZE, STATUS FROM FRANCHISE";
                using (var cmd = new System.Data.SqlClient.SqlCommand(query, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var franchise = new FranchiseModel
                            {
                                id = reader["ID"].ToString(),
                                name = reader["NAME"].ToString(),
                                email = reader["EMAIL"].ToString(),
                                contact_num = reader["CONTACT_NUM"].ToString(),
                                location = reader["LOCATION"].ToString(),
                                size = reader["SIZE"].ToString(),
                                status = reader["STATUS"].ToString()
                            };
                            franchiseData.Add(franchise);
                        }
                    }
                }
            }

            return franchiseData;
        }

        [HttpPost]
        public ActionResult NavigateToProducts()
        {
            return RedirectToAction("Home", "Products");
        }

        [HttpPost]
        public ActionResult SubmitFeedback(string name, string feedback, string rate)
        {
            // Define the connection string
            var connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\pc\Documents\Visual Studio 2012\Projects\JaCoolMVC\JaCoolMVC\App_Data\JACOOL_DB.mdf;Integrated Security=True";

            // Process the added data;
            using (var cn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var query = "INSERT INTO FEEDBACK (NAME, FBACKCOMMENT, FBACKRATE) VALUES (@NAME, @FBACKCOMMENT, @FBACKRATE)";
                using (var cmd = new System.Data.SqlClient.SqlCommand(query, cn))
                {
                    cn.Open();
                    cmd.Parameters.AddWithValue("@NAME", string.IsNullOrWhiteSpace(name) ? "Anonymous" : name);
                    cmd.Parameters.AddWithValue("@FBACKCOMMENT", feedback);
                    cmd.Parameters.AddWithValue("@FBACKRATE", rate);
                    cmd.ExecuteNonQuery();
                }
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SubmitFranchise(string name, string email, string contact, string location, string size)
        {
            var connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\pc\Documents\Visual Studio 2012\Projects\JaCoolMVC\JaCoolMVC\App_Data\JACOOL_DB.mdf;Integrated Security=True";

            using (var cn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string query = "INSERT INTO FRANCHISE (NAME, EMAIL, CONTACT_NUM, LOCATION, SIZE, STATUS) VALUES (@NAME, @EMAIL, @CONTACT_NUM, @LOCATION, @SIZE, @STATUS)";
                using (var cmd = new System.Data.SqlClient.SqlCommand(query, cn))
                {
                    cn.Open();

                    int status = 1;
                    cmd.Parameters.AddWithValue("@NAME", name);
                    cmd.Parameters.AddWithValue("@EMAIL", email);
                    cmd.Parameters.AddWithValue("@CONTACT_NUM", contact);
                    cmd.Parameters.AddWithValue("@LOCATION", location);
                    cmd.Parameters.AddWithValue("@SIZE", size);
                    cmd.Parameters.AddWithValue("@STATUS", status);
                    cmd.ExecuteNonQuery();
                }
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public JsonResult GetFranchiseData(string id)
        {
            var connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\pc\Documents\Visual Studio 2012\Projects\JaCoolMVC\JaCoolMVC\App_Data\JACOOL_DB.mdf;Integrated Security=True";

            using (var cn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string query = "SELECT NAME, EMAIL, CONTACT_NUM, LOCATION, SIZE, STATUS FROM FRANCHISE WHERE ID = @ID";
                using (var cmd = new System.Data.SqlClient.SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var data = new
                            {
                                name = reader["NAME"].ToString(), // change "name" to "NAME"
                                email = reader["EMAIL"].ToString(),
                                contact_num = reader["CONTACT_NUM"].ToString(),
                                location = reader["LOCATION"].ToString(),
                                size = reader["SIZE"].ToString(),
                                status = reader["STATUS"].ToString()
                            };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult UpdateFranchiseStatus(string id, string status)
        {
            var connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\pc\Documents\Visual Studio 2012\Projects\JaCoolMVC\JaCoolMVC\App_Data\JACOOL_DB.mdf;Integrated Security=True";

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "UPDATE FRANCHISE SET STATUS = @STATUS WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cn.Open();
                    cmd.Parameters.AddWithValue("@STATUS", status);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                    return Json(new { success = true, status = status });
                }
            }
        }
    }
}
