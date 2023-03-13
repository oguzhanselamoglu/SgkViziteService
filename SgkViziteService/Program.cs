using SgkViziteService.Services;
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

app.MapGet("/RaporOnay", async (string tckNo,string tarih,string vaka,string nitelikDurumu,string medulaRaporId, SgkService _service) =>
{
    var response = await _service.RaporOnayAsync(tckNo,tarih,vaka,nitelikDurumu,medulaRaporId);
    return response;
}).WithName("RaporOnay").WithOpenApi();

app.MapGet("/OnayliRaporlarDetay", async (string medulaRaporId, SgkService _service) =>
{
    var response = await _service.OnayliRaporlarDetayAsync(medulaRaporId);
    return response;
}).WithName("OnayliRaporlarDetay").WithOpenApi();

app.MapGet("/OnayliRaporlar", async (string tarih1,string tarih2, SgkService _service) =>
{
    var response = await _service.OnayliRaporlarTarihileAsync(tarih1,tarih2);
    return response;
}).WithName("OnayliRaporlar").WithOpenApi();

app.MapGet("/RaporOkunduKapat", async (string medulaRaporId, SgkService _service) =>
{
    var response = await _service.RaporOkunduKapatAsync(medulaRaporId);
    return response;
}).WithName("RaporOkunduKapat").WithOpenApi();
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}