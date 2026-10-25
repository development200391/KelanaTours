using KelanaTours.Models;
using KelanaTours.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
// Data tur dibaca dari Data/tours.json, konten lain dari Services/TourRepository.cs.
builder.Services.AddSingleton<ITourRepository, TourRepository>();

// Kontak, nomor WhatsApp, dan lokasi gambar diambil dari appsettings.json bagian "Site".
builder.Services.AddSingleton(builder.Configuration.GetSection("Site").Get<SiteOptions>() ?? new SiteOptions());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();   // kompatibel .NET 8 / 9 / 10
app.UseRouting();
app.UseAuthorization();

app.MapControllers();   // rute atribut: /our-tours, /tours/{slug}, /destinations, /contact, dst.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
