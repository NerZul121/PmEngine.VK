using PmEngine.Core.Extensions;
using PmEngine.Vk.WebApi.Actions;
using PmEngine.Vk;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

using ILoggerFactory loggerFactory = LoggerFactory.Create((a) => { a.AddConsole(); });
builder.Services.AddSingleton(loggerFactory);
builder.Services.AddSingleton<ILogger>(loggerFactory.CreateLogger(""));
builder.Services.AddLogging((lf) => lf.AddConsole());
builder.Services.AddVkModule();

builder.Services.AddPMEngine((e) =>
{
    e.Properties.InitializationAction = typeof(HelloWorldAction);
    e.Properties.DataProvider = PmEngine.Core.Enums.DataProvider.PG;
});

var app = builder.Build();
app.ConfigureEngine();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
