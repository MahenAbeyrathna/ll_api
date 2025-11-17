using Infrastructure.Data;
using LL.Application;

var builder = WebApplication.CreateBuilder(args);
var configurations = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbServices(configurations);
builder.Services.AddApplication(configurations);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add built-in logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
