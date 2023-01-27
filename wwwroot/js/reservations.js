"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/reservationHub").build();



connection.start().then(function () {

    //alert("Uspela konekcija!");
    InvokeReservations();
}).catch(function (err) {
    return console.log(err.toString());
});


function InvokeReservations() {
    
    connection.invoke("SendReservations").catch(function (err) {
        return console.log(err.toString());
    });
}


connection.on("ReceiveReservations", function (reservations) {
    BindReservations(reservations);
});

function BindReservations(reservations) {

   



    //var username=JSON.parse(localStorage.getItem("loggedIn"));
    var username="";
    if (document.getElementById("RegUserUsername")!=null)
    username = document.getElementById("RegUserUsername").innerHTML;


    let res_parsed = JSON.parse(reservations);

    if (username==="")
    $('#ReservationsTableAgent').empty();
    else
    $('#ReservationsUserTable').empty();

    var tr;
    var tr2;
    $.each(res_parsed, function (index, reservation) {

        if (reservation.Status === 'Cekanje' && username==="") { 
        tr = $('<tr/>');
        tr.append(`<td>${reservation.MestoOd}</td>`);
        tr.append(`<td>${reservation.MestoDo}</td>`);
        tr.append(`<td>${reservation.Datum}</td>`);
        tr.append(`<td>${reservation.KorIme}</td>`);
        tr.append(`<td>${reservation.BrMesta}</td>`);

        tr.append(`<td> <a class="btn btn-primary btn-sm" href="/Agent/AcceptReservation?id=${reservation.Id}">Odobrite</a></td>`);
        tr.append(`<td> <a class="btn btn-primary btn-sm" href="/Agent/DeclineReservation?id=${reservation.Id}">Odbite</a></td>`);

            $('#ReservationsTableAgent').append(tr);
        }

        if (reservation.KorIme === username)
        {
            tr2 = $('<tr/>');
            tr2.append(`<td>${reservation.MestoOd}</td>`);
            tr2.append(`<td>${reservation.MestoDo}</td>`);
            tr2.append(`<td>${reservation.Datum}</td>`);
            tr2.append(`<td>${reservation.BrPresedanja}</td>`);
            tr2.append(`<td>${reservation.BrMesta}</td>`);
            tr2.append(`<td>${reservation.Status}</td>`);
            $('#ReservationsUserTable').append(tr2);
        }
        


    });




    event.preventDefault();

    

}