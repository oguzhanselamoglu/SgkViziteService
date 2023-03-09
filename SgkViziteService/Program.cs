using SgkViziteService.Services;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Configuration.GetSection("AzureConnectionStrings")["CloudConStr"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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


app.MapGet("/login", async () =>
{

    SgkService service = new SgkService("https://uyg.sgk.gov.tr/Ws_Vizite/services/ViziteGonder", "", "", "");
    var response = await service.LogiAsyncn();
    return response;
}).WithName("login").WithOpenApi();


app.MapGet("/isyeriGoruntu", async () =>
{

    SgkService service = new SgkService("https://uyg.sgk.gov.tr/Ws_Vizite/services/ViziteGonder", "", "", "");

    var response = await service.IsverenIletisimBilgileriGoruntuAsync();
    return response;
}).WithName("isyeriGoruntu").WithOpenApi();
app.MapGet("/Raporarama", async (string tckNo) =>
{

    SgkService service = new SgkService("https://uyg.sgk.gov.tr/Ws_Vizite/services/ViziteGonder", "", "", "");

    var response = await service.RaporAramaKimlikNoAsync(tckNo);
    return response;
}).WithName("Raporarama").WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
