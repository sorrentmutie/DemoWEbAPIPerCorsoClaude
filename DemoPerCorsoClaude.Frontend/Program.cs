using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using DemoPerCorsoClaude.Frontend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5082/";
builder.Services.AddHttpClient("ProductsApi", client =>
    client.BaseAddress = new Uri(apiBaseUrl));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
