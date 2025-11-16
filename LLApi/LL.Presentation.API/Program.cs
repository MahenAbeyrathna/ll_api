using Infrastructure.Data;
using LL.Application;
using LL.Application.Common.Interfaces.Services.DataAPIService;
using LL.Infrastructure.Integrations.DataAPI;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configurations = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbServices();
builder.Services.AddApplication(configurations);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataService, DataAPIService>();
// Add built-in logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "LL Data API Documentation";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Hello from the logger!");
    return "Logged!";
});

app.Run();
