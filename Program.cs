using Microsoft.EntityFrameworkCore;
using Villafy_Api;
using Villafy_Api.Data;
using Villafy_Api.Logging;

var builder = WebApplication.CreateBuilder(args);
///AppDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultSQLConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
// Add services to the container.

///Add AutoMapper

builder.Services.AddAutoMapper(typeof(MappingConfig));
///// Default log implementation
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("Log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, Logging>();
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

app.Run();
