using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;

namespace AvioApp.Pages.Agent
{
    public class IndexModel : PageModel
    {
        public String Username = "";
        public List<FlightInfo> flights = new List<FlightInfo>();

        public String PlaceFrom = "";
        public String PlaceTo = "";
        public String date = "";
        public String min_date = DateTime.Now.AddDays(3).ToString("d",new CultureInfo("pt-BR"));
        public int transfers = 0;
        public int capacity = 0;

        public String error_message = "";
        public String success_message = "";



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
                                flight.taken_seats=reader.GetInt32(6);
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
            transfers = Convert.ToInt32( Request.Form["Transfers"]);
            capacity = Convert.ToInt32(Request.Form["Capacity"]);

            if(PlaceFrom.Equals(PlaceTo))
            {
                error_message = "Mesto polaska mora da se razlikuje od mesta dolaska!";
                OnGet();
                return;
            }

            try
            {
                String connection_string = "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;Trust Server Certificate=true";

                using(SqlConnection connection = new SqlConnection(connection_string))
                {
                    connection.Open();

                    String sql = "Insert into Let VALUES(@PlaceFrom,@PlaceTo,@Date,@Transfer,@Capacity,'Aktivan')";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@PlaceFrom", PlaceFrom);
                        cmd.Parameters.AddWithValue("@PlaceTo", PlaceTo);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@Transfer", transfers);
                        cmd.Parameters.AddWithValue("@Capacity", capacity);

                        cmd.ExecuteNonQuery();

                        success_message = "Uspešno ste dodali let!";


                    }

                }

            }
            catch(Exception ex)
            {

            }



            OnGet();



        }

    }

    public class FlightInfo
    {
        public int id;
        public string place_from;
        public string place_to;
        public string date;
        public int capacity;
        public int transfers;
        public int taken_seats;
        public string status;
    }
}
