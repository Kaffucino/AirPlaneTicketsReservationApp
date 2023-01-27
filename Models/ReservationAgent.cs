namespace AvioApp.Models
{
    public class ReservationAgent
    {
        public int Id { get; set; }
        public string MestoOd { get; set; }
        public string MestoDo { get; set; }
        public string Datum { get; set; }
        public string KorIme { get; set; }
        public int BrMesta { get; set; }
        public int BrPresedanja { get; set; }

        public string Status { get; set; }

    }
}
