using AvioApp.Pages.Agent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography.Xml;

namespace AvioApp.Pages.Admin
{
    public class IndexModel : PageModel
    {

        public String Username = "";
        public String error_message = "";
        public String success_message = "";
        public String PlaceFrom = "";
        public String PlaceTo = "";
        public String date = "";

        public List<FlightInfo> flights = new List<FlightInfo>();

        public void OnGet()
        {
            
            Username = Request.Query["username"];

            try
            {
                String connection_string = "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;Trust Server Certificate=true";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    connection.Open();

                    String sql = "Select L.Id,L.MestoOd,L.MestoDo,L.Datum,L.BrPresedanja,L.Kapacitet,COALESCE((Select SUM(BrMesta) from Rezervacija R2 where R2.IdLet=L.Id and R2.Status='Odobreno' group by R2.IdLet),0),L.Status from Let L  left join Rezervacija R on(L.Id=R.IdLet) group by L.Id, MestoOd,MestoDo,Datum,BrPresedanja,L.Kapacitet,L.Status";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FlightInfo flight = new FlightInfo();
                                flight.id = reader.GetInt32(0);
                                flight.place_from = reader.GetString(1);
                                flight.place_to = reader.GetString(2);
                                flight.date = reader.GetDateTime(3).ToShortDateString();
                                flight.transfers = reader.GetInt32(4);
                                flight.capacity = reader.GetInt32(5);
                                flight.taken_seats = reader.GetInt32(6);
                                flight.status = reader.GetString(7);


                                flights.Add(flight);

                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void OnPost()
        {
            PlaceFrom = Request.Form["PlaceFrom"];
            PlaceTo = Request.Form["PlaceTo"];
            date = Convert.ToDateTime(Request.Form["Date"]).ToShortDateString();

            try
            {
                String connection_string = "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;Trust Server Certificate=true";

                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    connection.Open();

                    String sql = "Select Id from Let where MestoOd=@PlaceFrom and MestoDo=@PlaceTo and Datum=@date and Status='Aktivan'";
                    List<int> flight_ids = new List<int>();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@PlaceFrom", PlaceFrom);
                        cmd.Parameters.AddWithValue("@PlaceTo", PlaceTo);
                        cmd.Parameters.AddWithValue("@date", date);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                flight_ids.Add(reader.GetInt32(0));
                                
                            }
                            if(flight_ids.Count ==0)
                            {
                                error_message = "Let za navedeni datum ne postoji ili je već otkazan!";

                                return;

                            }
                            

                        }

                        



                    }
                    if (error_message.Length == 0)
                    {
                        sql = "Update Let set Status='Otkazan' where MestoOd=@PlaceFrom and MestoDo=@PlaceTo and Datum=@date";
                        using(SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@PlaceFrom", PlaceFrom);
                            cmd.Parameters.AddWithValue("@PlaceTo", PlaceTo);
                            cmd.Parameters.AddWithValue("@date", date);
                            cmd.ExecuteNonQuery();

                        }

                        sql = "Update Rezervacija set Status='Otkazano' where IdLet=@id";

                        foreach(int id in flight_ids)
                        {
                            using(SqlCommand cmd=new SqlCommand(sql, connection))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }

                        }

                        success_message = "Uspešno ste otkazali let!";

                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }
            OnGet();



        }

       
    }
}
