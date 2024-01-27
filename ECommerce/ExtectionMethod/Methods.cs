using Azure.Storage.Blobs;
using BusinessLayer.Repository;
using BusinessLayer.RepositoryImplementation;
using DataAcessLayer.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Reflection;
using System.Text;

namespace ECommerce.ExtectionMethod
{
    public static class Methods
    {
        #region Database Conection

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EcDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection")
            , dbOpt => dbOpt.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
);

            return services;
        }

        public static IServiceCollection AddDatabaseAzure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EcDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DefaultConnectionAzure")
            , dbOpt => dbOpt.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
);

            return services;
        }

        #endregion Database Conection

        #region Dependency injection

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IRoles, RolesRepo>();
            services.AddTransient<IUser, UsersRepo>();
            services.AddTransient<ICategories, CategoriesRepo>();
            services.AddTransient<IProduct, ProductsRepo>();
            services.AddTransient<IOrders, OrderRepo>();
            services.AddTransient<IMailService, MailService>();

            return services;
        }

        #endregion Dependency injection

        #region JwT autherntication

        //?--------------------------JWT authentication ---------------------------------------------

        public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
            return services;
        }

        #endregion JwT autherntication

        #region NewtonSoftJson

        public static IServiceCollection AddNewtonJson(this IServiceCollection services)
        {
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           );
            return services;
        }

        #endregion NewtonSoftJson

        #region Swagger Api

        public static IServiceCollection Swagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                 
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            }
);
            return services;
        }

        #endregion Swagger Api

        #region Blobstorage

        public static IServiceCollection Blobservice(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ =>
            {
                return new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
            });

            return services;
        }

        #endregion Blobstorage
    }
}