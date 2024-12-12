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
            // Konfigurera Serilog för SQL Server Express
            //var columnOptions = new ColumnOptions
            //{
            //    TimeStamp = { ColumnName = "Timestamp", DataType = System.Data.SqlDbType.DateTime },
            //    Level = { ColumnName = "Level", DataType = System.Data.SqlDbType.NVarChar },
            //    Message = { ColumnName = "Message", DataType = System.Data.SqlDbType.NVarChar },
            //    Exception = { ColumnName = "Exception", DataType = System.Data.SqlDbType.NVarChar },
            //    Properties = { ColumnName = "Properties", DataType = System.Data.SqlDbType.NVarChar }
            //};

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.MSSqlServer(
            //        connectionString: "Server=MSI\\SQLEXPRESS;Database=TobyServer;Trusted_Connection=true;TrustServerCertificate=True;",
            //        tableName: "Logs",
            //        autoCreateSqlTable: true,
            //        columnOptions: columnOptions)
            //    .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Lägg till loggning och Serilog
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            // Lägg till tjänster
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<Realdatabase>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(connectionString);

            // JWT och autentisering
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

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

            builder.Services.AddAuthorization();

            // Swagger
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

            var app = builder.Build();

            // Kör migrations och populera databasen
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Realdatabase>();

                try
                {
                    dbContext.Database.Migrate(); // Säkerställ att migrations körs
                    Realdatabase.SeedDatabase(dbContext); // Populera databasen
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ett fel uppstod när databasen skulle populeras.");
                }
            }

            // Konfigurera HTTP-request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

