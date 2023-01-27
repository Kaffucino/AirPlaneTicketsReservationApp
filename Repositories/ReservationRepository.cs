using AvioApp.Models;
using AvioApp.Pages.Agent;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AvioApp.Repositories
{
    public class ReservationRepository
    {
        string connection_string;
        
        public ReservationRepository(string connection_string)
        {
            this.connection_string = connection_string;
        }

        public List<ReservationAgent> GetReservations()
        {
            List<ReservationAgent> reservations = new List<ReservationAgent>();
            ReservationAgent reservation;

            var data = GetReservationsFromDb();

            foreach(DataRow row in data.Rows)
            {
                reservation = new ReservationAgent();
                reservation.Id = Convert.ToInt32(row["Id"]);
                reservation.MestoOd = row["MestoOd"].ToString();
                reservation.MestoDo = row["MestoDo"].ToString();
                reservation.Datum = Convert.ToDateTime(row["Datum"]).ToShortDateString();
                reservation.KorIme = row["KorIme"].ToString();
                reservation.BrMesta = Convert.ToInt32(row["BrMesta"]);
                reservation.BrPresedanja= Convert.ToInt32(row["BrPresedanja"]);
                reservation.Status= row["Status"].ToString();
                reservations.Add(reservation);

            }

            return reservations;
        }

        public DataTable GetReservationsFromDb()
        {
            var query = "select R.Id,MestoOd,MestoDo,Datum,K.KorIme,BrMesta,BrPresedanja,R.Status from Let l join Rezervacija R on(l.Id=R.IdLet) join Korisnici K on(R.IdKor=K.Id)";
            DataTable data_table = new DataTable();

            using(SqlConnection connection= new SqlConnection(connection_string))
            {

                try
                {
                    connection.Open();
                    using(SqlCommand command=new SqlCommand(query, connection))
                    {
                       using(SqlDataReader reader= command.ExecuteReader())
                        {
                            data_table.Load(reader);
                        }

                    }
                    return data_table;
                }catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }   
}
