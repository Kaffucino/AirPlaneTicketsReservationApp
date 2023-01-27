namespace AvioApp.Models
{
    public class Rezervacija
    {
        public int Id { get; set; }
        public int IdLet { get; set; }
        public int IdKor { get; set; }
        public int BrMesta { get; set; }
        public string Status { get; set; }
    }
}
