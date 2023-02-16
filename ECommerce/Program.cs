using ECommerce.ExtectionMethod;
using ECommerce.GlobalException;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDatabase(builder.Configuration)
    .AddServices()
    .AddJWT(builder.Configuration)
    .AddNewtonJson()
    .Swagger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//?----------------------- Serilog Configuraion  --------------------------------

#region Serilog Configuration

string con = builder.Configuration.GetConnectionString("DefaultConnection");
string table = "Logs";

//var _Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.MSSqlServer(con, table).CreateLogger();
//builder.Logging.AddSerilog(_Logger);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.MSSqlServer(con, table).CreateLogger();

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