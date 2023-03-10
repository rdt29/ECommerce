using AutoMapper;
using BusinessLayer.RepositoryImplementation;
using ECommerce.ExtectionMethod;
using ECommerce.GlobalException;
using Microsoft.Extensions.Azure;
using SendGrid.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddDatabase(builder.Configuration)
    //.AddDatabaseAzure(builder.Configuration)
    .AddServices()
    .AddJWT(builder.Configuration)
    .AddNewtonJson()
    .Swagger()
    .Blobservice(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
//?------------------------COres-------------------------------------------------------
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                      });
});

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
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["ConnectionStrings:AzureBlobStorage:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["ConnectionStrings:AzureBlobStorage:queue"], preferMsi: true);
});

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.MSSqlServer(con, table).CreateLogger();

#endregion Serilog Configuration

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
//? GLobal Exception handling----------------------------
app.UseMiddleware<GlobalException>();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();