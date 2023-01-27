using AvioApp.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Text.Json;

namespace AvioApp.Hubs
{
    public class ReservationHub : Hub
    {
        ReservationRepository reservationRep;

        public ReservationHub(IConfiguration configuration)
        {
            var connection_string = "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;Trust Server Certificate=true";
            reservationRep= new ReservationRepository(connection_string);
        }

        public async Task SendReservations()
        {   

            var reservations=reservationRep.GetReservations();


            //Debug.WriteLine(reservations.ElementAt(0).id + " " + reservations.ElementAt(0).place_from + " " + reservations.ElementAt(0).place_to);

            String reservations_serialized=JsonSerializer.Serialize(reservations);

            Debug.WriteLine(reservations_serialized);

            await Clients.All.SendAsync("ReceiveReservations", reservations_serialized );
        }




    }
}
