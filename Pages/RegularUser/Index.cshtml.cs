using AvioApp.Pages.Agent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace AvioApp.Pages.RegularUser
{
    public class IndexModel : PageModel
    {

        public List<FlightInfo> flights=new List<FlightInfo>();
        public String Username = "";

        public String PlaceFrom = "";
        public String PlaceTo = "";
        
        public Boolean Transfer = false;

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

                                if(flight.taken_seats != flight.capacity)
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

            if (Request.Form["Transfer"].Equals("on"))
                Transfer = true;

            Debug.WriteLine(Transfer);
            
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

                                if (flight.taken_seats != flight.capacity)
                                {

                                    if (Transfer && flight.transfers > 0)
                                        continue;

                                    if (!PlaceFrom.Equals("/") && !flight.place_from.Equals(PlaceFrom))
                                        continue;

                                    if (!PlaceTo.Equals("/") && !flight.place_to.Equals(PlaceTo))
                                        continue;

                                    flights.Add(flight);

                                }

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
    }
}
