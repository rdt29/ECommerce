using AutoMapper;
using BusinessLayer.RepositoryImplementation;
using ECommerce.ExtectionMethod;
using ECommerce.GlobalException;
using Microsoft.EntityFrameworkCore.Metadata;
using SendGrid.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDatabase(builder.Configuration)
    .AddServices()
    .AddJWT(builder.Configuration)
    .AddNewtonJson()
    .Swagger()
    .Blobservice(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//?--------------------------------------SendGrid-------------------------------------------------------

#region sendgrid

builder.Services.AddSendGrid(options =>
{
    options.ApiKey = builder.Configuration
    .GetSection("SendGridEmailSettings").GetValue<string>("APIKey");
});

#endregion sendgrid

//?----------------------------smtp mail---------------------------------------------------------------
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

//?----------------------------------Automapper-------------------------------------
builder.Services.AddAutoMapper(typeof(Program));

//var automapper = new MapperConfiguration(item => item.AddProfile(new AutoMappers()));

//IMapper mapper = automapper.CreateMapper();

//builder.Services.AddSingleton(mapper);

//?----------------------- Serilog Configuraion  --------------------------------

#region Serilog Configuration

string con = builder.Configuration.GetConnectionString("DefaultConnection");
string table = "Logs";

var _logger = new LoggerConfiguration()
.MinimumLevel.Debug()
    .WriteTo.MSSqlServer(con, table).CreateLogger();
builder.Logging.AddSerilog(_logger);

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.MSSqlServer(con, table).CreateLogger();

#endregion Serilog Configuration

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//? GLobal Exception handling----------------------------
app.UseMiddleware<GlobalException>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();