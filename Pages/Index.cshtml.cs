using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace AvioApp.Pages
{
    public class IndexModel : PageModel
    {


        public String Username = "";
        public String Password="";
        public String error_message="";

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            Username = Request.Form["Username"];
            Password = Request.Form["Password"];


            if(Username.Equals("") || Password.Equals(""))
            {
                error_message = "Popunite sva polja!";
                return;
            }

            try
            {

                String connection_string= "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;Trust Server Certificate=true";

                using (SqlConnection con = new SqlConnection(connection_string))
                {
                    con.Open();
                    String sql = "select Id,Tip from Korisnici where KorIme=@username and Sifra=@password";

                    using(SqlCommand command=new SqlCommand(sql,con))
                    {
                        command.Parameters.AddWithValue("@username", Username);
                        command.Parameters.AddWithValue("@password", Password);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var id = reader.GetInt32(0);
                                var tip = reader.GetString(1);

                                if (tip.Equals("admin"))
                                {
                                    Response.Redirect("/Admin/Index?username=" + Username);
                                } else if (tip.Equals("agent"))
                                {
                                    Response.Redirect("/Agent/Index?username=" + Username);
                                }
                                else //posetilac
                                {

                                    Response.Redirect("/RegularUser/Index?username=" + Username);

                                }

                            }
                            else
                            {
                                error_message = "Pogrešni kredencijali!";
                                return;
                            }

                        }

                    }
                }


            }
            catch(Exception ex)
            {
                error_message = ex.Message;
                return;
            }


        }

    }
}