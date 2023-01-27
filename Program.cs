using AvioApp.Hubs;
using AvioApp.MiddlewareExtensions;
using AvioApp.SubscribeTableDependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddSingleton<ReservationHub>();
builder.Services.AddSingleton<SubscribeReservationsTableDependencies>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapHub<ReservationHub>("/reservationHub"); 

app.MapRazorPages();

app.UseReservationsTableDependency();
app.Run();
