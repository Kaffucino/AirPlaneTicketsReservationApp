using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AvioApp.Pages.Admin
{   
    public class AddUsersModel : PageModel
    {


        public String AdminUserName = "";

        [BindProperty]
        public String NewUsername { get; set; } = "";
        [BindProperty]
        public String Password { get; set; } = "";
        [BindProperty]
        public String PasswordRepeat { get; set; } = "";

        public String error_message = "";
        public String success_message = "";


        [BindProperty]
        public String Type { get; set; }
       

        public void OnGet()
        {
            AdminUserName = Request.Query["username"];

           

        }


        public void OnPost()
        {
            AdminUserName = Request.Query["username"];

            if (NewUsername == null || Password == null || PasswordRepeat == null || Type == null || NewUsername.Equals("") || Password.Equals("") || PasswordRepeat.Equals(""))
            {
                error_message = "Popunite sva polja!";

                return;
            }

            if(!Password.Equals(PasswordRepeat))
            {
                error_message = "Šifru koju ste ponovo uneli mora biti ista kao nova šifra!";
                NewUsername = "";
                Password = "";
                PasswordRepeat = "";

                return;

            }
            try
            {
                String connection_string = "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;Trust Server Certificate=true";
                using(SqlConnection connection= new SqlConnection(connection_string))
                {
                    connection.Open();

                    String sql = "Select * from Korisnici where KorIme=@NewUsername";
                    using(SqlCommand command=new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NewUsername", NewUsername);
                        using(SqlDataReader reader= command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                error_message = "Korisničko ime je zauzeto!";
                                NewUsername = "";
                                Password = "";
                                PasswordRepeat = "";

                                return;
                            }
                        }
                    }

                    sql = "Insert into Korisnici VALUES(@Username,@Password,@Type)";

                    using(SqlCommand cmd=new SqlCommand(sql,connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", NewUsername);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@Type", Type);

                        cmd.ExecuteNonQuery();

                        success_message = "Uspešno ste dodali korisnika!";
                        NewUsername = "";
                        Password = "";
                        PasswordRepeat = "";
                    }


                }


            }
            catch (Exception ex)
            {

            }




           
        }
    }

   

}
