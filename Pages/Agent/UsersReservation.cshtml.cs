using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace AvioApp.Pages.Agent
{
    public class UsersReservationModel : PageModel
    {
        public String Username = "";
        public List<ReservationInfo> reservations= new List<ReservationInfo>();
        public String error_message = "";

        
    

       
        public void OnGet()
        {
            Username = Request.Query["username"];
            String error = Request.Query["errorMessage"];
            if (error != null)
                error_message = error;
            

        }
    }


   

    public class ReservationInfo
    {
        public int id;
        public string place_from;
        public string place_to;
        public string date;
        public string user;
        public int num_of_seats;
        public string status;
        public int transfers;
    }
}
