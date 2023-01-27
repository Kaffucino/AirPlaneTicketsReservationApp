using AvioApp.Pages.Agent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace AvioApp.Pages.RegularUser
{
    public class MyReservationsModel : PageModel
    {
        public String Username="";
        public List<ReservationInfo> reservations=new List<ReservationInfo>();
        public void OnGet()
        {
            Username = Request.Query["username"];

          

        }
    }


    public class ReservationInfo
    {
        public int id;
        public string place_from;
        public string place_to;
        public string date;
        public int transfer;
        public int reservations;
        public string status;
    }
}
