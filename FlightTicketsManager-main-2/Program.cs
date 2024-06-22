using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SamolotContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SamolotContext")));

// Configure LotniskoContext (assuming you have another context)
builder.Services.AddDbContext<LotniskoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LotniskoContext")));

// Configure LotContext
builder.Services.AddDbContext<LotContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LotContext")));

builder.Services.AddDbContext<BiletContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BiletContext")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lot}/{action=Index}/{id?}");

app.Run();
