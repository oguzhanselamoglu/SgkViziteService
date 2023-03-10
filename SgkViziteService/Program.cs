﻿using SgkViziteService.Services;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<SgkService>(s => new SgkService(builder.Configuration.GetSection("Sgk")["Url"],
    builder.Configuration.GetSection("Sgk")["KullaniciAdi"],
    builder.Configuration.GetSection("Sgk")["IsyeriKodu"],
    builder.Configuration.GetSection("Sgk")["IsyeriSifresi"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsEnvironment("Local"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();


app.MapGet("/login", async (SgkService _service) =>
{
    var response = await _service.LogiAsyncn();
    return response;
}).WithName("login").WithOpenApi();


app.MapGet("/isyeriGoruntu", async (SgkService _service) =>
{
    var response = await _service.IsverenIletisimBilgileriGoruntuAsync();
    return response;
}).WithName("isyeriGoruntu").WithOpenApi();
app.MapGet("/RaporAramaKimlikNo", async (string tckNo, SgkService _service) =>
{
    var response = await _service.RaporAramaKimlikNoAsync(tckNo);
    return response;
}).WithName("RaporAramaKimlikNo").WithOpenApi();
app.MapGet("/RaporAramaTarihile", async (string tarih, SgkService _service) =>
{
    var response = await _service.RaporAramaTarihileAsync(tarih);
    return response;
}).WithName("RaporAramaTarihile").WithOpenApi();
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}