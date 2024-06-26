using MenedzerZakupuBiletow.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RezerwacjaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RezerwacjaContext")));
builder.Services.AddDbContext<PasazerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PasazerContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Bilet/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Bilet}/{action=Index}/{id?}");

app.Run();
