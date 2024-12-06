using ApplicationBook;
using Infrastructur;
using Infrastructur.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Microsoft.Extensions.Hosting;

namespace Web_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Konfigurera Serilog f�r SQL Server Express
            var columnOptions = new ColumnOptions
            {
                // Specifika kolumner f�r loggar
                TimeStamp = { ColumnName = "Timestamp", DataType = System.Data.SqlDbType.DateTime },
                Level = { ColumnName = "Level", DataType = System.Data.SqlDbType.NVarChar },
                Message = { ColumnName = "Message", DataType = System.Data.SqlDbType.NVarChar },
                Exception = { ColumnName = "Exception", DataType = System.Data.SqlDbType.NVarChar },
                Properties = { ColumnName = "Properties", DataType = System.Data.SqlDbType.NVarChar }
            };

            Log.Logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: "Server=MSI\\SQLEXPRESS;Database=TobyServer;Trusted_Connection=true;TrustServerCertificate=True;", 
                    tableName: "Logs",  // Namnet p� loggtabellen
                    autoCreateSqlTable: true,  // Skapa tabellen automatiskt om den inte finns
                    columnOptions: columnOptions)  // Anv�nd kolumninst�llningar f�r loggdata
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // L�gg till loggning f�r fels�kning (valfritt)
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();  // Anv�nd Serilog f�r loggning

            // L�gg till tj�nster till DI-kontainern
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            byte[] secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web_API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorize with your bearer token that generates when you login",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // L�gg till Application-tj�nster
            builder.Services.AddApplication();

            // H�mta anslutningsstr�ngen fr�n konfiguration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddInfrastructure(connectionString);

            // L�gg till MediatR om det inte redan �r gjort i AddApplication
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            // Konfigurera HTTP-request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Viktigt att anropa UseAuthentication f�re UseAuthorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
