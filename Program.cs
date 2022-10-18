using GestionPrestamosPersonales.BLL;
using GestionPrestamosPersonales.DAL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Radzen;
 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<NotificationService>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var ConStr = builder.Configuration.GetConnectionString("ConStr");

builder.Services.AddDbContext<Contexto>(con =>
    con.UseSqlite(ConStr)
);
builder.Services.AddTransient<OcupacionesBLL>();
builder.Services.AddTransient<PersonasBLL>();
builder.Services.AddTransient<PrestamosBLL>();
builder.Services.AddTransient<PagosBLL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
