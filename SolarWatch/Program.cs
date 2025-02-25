using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Context;
using SolarWatch.Repositories;
using SolarWatch.Services;
using SolarWatch.Services.Authentication;


namespace SolarWatch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // for appSettings.json
            var configuration = builder.Configuration; // this here in program.cs to ready out connection string
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")); // this so it can be used anywhere NOT in program.cs

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
            });

            builder.Services.AddSingleton<ISolarDataProvider, SolarDataProvider>();
            builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
            builder.Services.AddScoped<ISolarWatchRepository, SolarWatchRepository>();
            builder.Services.AddScoped<ISolarDataService, SolarDataService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<AuthenticationSeeder>();
            builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();


            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "apiWithAuthBackend",
                        ValidAudience = "apiWithAuthBackend",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("!SomethingSecret!!SomethingSecret!")
                        ),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var tokenBlacklistService = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
                            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                            if (tokenBlacklistService.IsTokenBlacklisted(token))
                            {
                                context.Fail("This token is blacklisted.");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });



            if (!builder.Environment.IsEnvironment("Test"))
            {
                builder.Services.AddDbContext<SolarWatchApiContext>(options =>
                {
                    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_DOCKER")
                                           ?? configuration["AppSettings:CONNECTION_STRING_DOCKER"];

                    options.UseNpgsql(connectionString);
                });
            }


            builder.Services
                .AddIdentityCore<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SolarWatchApiContext>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                // register appsettings 
                var config = scope.ServiceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

                // do automatic migraions
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<SolarWatchApiContext>();
                    dbContext.Database.Migrate(); // Apply migrations
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while applying database migrations.");
                }

                // run role seeder
                var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
                authenticationSeeder.AddRoles();
                authenticationSeeder.AddAdmin();
            }
            

            // Configure the HTTP request pipeline.
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
