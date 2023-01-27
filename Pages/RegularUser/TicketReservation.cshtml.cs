using AvioApp.Pages.Agent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace AvioApp.Pages.RegularUser
{
    public class TicketReservationModel : PageModel
    {

        public String Username="";
        public FlightInfo flight=new FlightInfo();
        public int number_of_reservations = 0;
        public String success_message = "";
        public String error_message = "";
        private DateTime flight_date=new DateTime();

        public void OnGet()
        {

           int id= Convert.ToInt32(Request.Query["id"]);
            Username = Request.Query["username"];

            try
            {
                String connection_string = "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;Trust Server Certificate=true";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    connection.Open();

                    String sql = "Select L.Id,L.MestoOd,L.MestoDo,L.Datum,L.BrPresedanja,L.Kapacitet,COALESCE((Select SUM(BrMesta) from Rezervacija R2 where R2.IdLet=L.Id and R2.Status='Odobreno' group by R2.IdLet),0) from Let L  left join Rezervacija R on(L.Id=R.IdLet) where L.Id=@Id group by L.Id, MestoOd,MestoDo,Datum,BrPresedanja,L.Kapacitet";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id",id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                flight.id = reader.GetInt32(0);
                                flight.place_from = reader.GetString(1);
                                flight.place_to = reader.GetString(2);
                                flight.date = reader.GetDateTime(3).ToShortDateString();
                                flight.transfers = reader.GetInt32(4);
                                flight.capacity = reader.GetInt32(5);
                                flight.taken_seats = reader.GetInt32(6);

                                flight_date=reader.GetDateTime(3);
                            }
                        }
                    }
                }



            }
            catch(Exception ex)
            {

            }

        }
        public void OnPost()
        {
            OnGet();

            DateTime now=DateTime.Now;   
            TimeSpan difference = flight_date - now;
            if(difference.Days < 3)
            {
                error_message = "Nemoguće je rezervisati mesto na letu koji je za manje od 3 dana!";
                return;
            }
            var user_id = 0;
            number_of_reservations = Convert.ToInt32(Request.Form["Reservations"]);

            try
            {
                String connection_string = "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;Trust Server Certificate=true";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    connection.Open();
                 String sql = "Select Id from Korisnici where KorIme=@korime";
                 using (SqlCommand command = new SqlCommand(sql, connection))
                  {
                        command.Parameters.AddWithValue("@korime", Username);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read()) { 
                            
                                user_id=reader.GetInt32(0);
                            
                            }
                        }
                  }
                    sql = "Insert into Rezervacija VALUES(@IdLet,@IdKor,@BrMesta,'Čekanje')";

                    using(SqlCommand command1 = new SqlCommand(sql,connection))
                    {
                        command1.Parameters.AddWithValue("@IdLet", flight.id);
                        command1.Parameters.AddWithValue("@IdKor", user_id);
                        command1.Parameters.AddWithValue("@BrMesta", number_of_reservations);

                        command1.ExecuteNonQuery();

                        success_message = "Uspešno ste izvršili rezervaciju!";

                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }


        }

    }
}
