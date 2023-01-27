using AvioApp.Hubs;
using AvioApp.Models;
using TableDependency.SqlClient;

namespace AvioApp.SubscribeTableDependencies
{
    public class SubscribeReservationsTableDependencies
    {
        SqlTableDependency<Rezervacija> tableDependency;
        ReservationHub reservationHub;

        public SubscribeReservationsTableDependencies(ReservationHub reservationHub)
        {
            this.reservationHub= reservationHub;
        }

        public void subscribeTableDependency()
        {
            string connection_string = "Data Source=DESKTOP-L9H288F;Initial Catalog=AvioApp;Integrated Security=True;";
            tableDependency = new SqlTableDependency<Rezervacija>(connection_string);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }
        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Rezervacija> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                reservationHub.SendReservations();

            }
        }
        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Rezervacija)} SQLTable Error : {e.Error.Message}");

        }
    }
}
