using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // current workaround for port forwarding in codespaces
    // https://github.com/dotnet/aspnetcore/issues/57332
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Servers = [];
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

string GetSummaryFromTemperature(int temperatureC)
{
    return temperatureC switch
    {
        < 0 => "Freezing",
        < 5 => "Bracing",
        < 10 => "Chilly",
        < 15 => "Cool",
        < 20 => "Mild",
        < 25 => "Warm",
        < 30 => "Balmy",
        < 35 => "Hot",
        < 40 => "Sweltering",
        _ => "Scorching"
    };
}

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
    {
        var temp = Random.Shared.Next(-20, 55);
        return new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            temp,
            GetSummaryFromTemperature(temp)
        );
    })
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
