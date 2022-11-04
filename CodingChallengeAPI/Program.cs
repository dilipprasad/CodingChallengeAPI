using CodingChallengeAPI.Middleware;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.Logging.Console;

namespace CodingChallengeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Console.WriteLine("Fetching Environment Variable ASPNETCORE_ENVIRONMENT =" + env);

            // Appsetting file name based on Environment 
            var appSettingFileName = string.Join(".", "appsettings", env, "json");
            Console.WriteLine("AppSetting FileName =" + appSettingFileName);



            //Reding the configuration from Appsettings file
            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile(appSettingFileName).Build();

            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Enabling Memory Cache
            builder.Services.AddMemoryCache();

            //Adding Automapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            //Adding Custom middlewre to handle the logs
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
            });

            var logger = loggerFactory.CreateLogger<Program>();

            builder.Services.AddSingleton(typeof(CodingChallenge.Logging.Interface.ILogging<>), typeof(CodingChallenge.Logging.Logger<>));

            //Adding other mappings
            builder.Services.AddScoped<CodingChallenge.DataLayer.DataLayer.IDataSource, CodingChallenge.DataLayer.DataLayer.DataSource>(); //Normally this will be the DB Interface, testing interface will be used in mock test projects
            builder.Services.AddScoped<CodingChallenge.DataLayer.DataAdaptor.IDataLayer, CodingChallenge.DataLayer.DataAdaptor.DataLayer>();
            builder.Services.AddScoped<CodingChallenge.DataLayer.DataProvider.Interfaces.ICityDataProvider, CodingChallenge.DataLayer.DataProvider.CityDataProvider>();
            builder.Services.AddScoped<CodingChallenge.DataLayer.ObjectFactory.Interfaces.IObjectDataFactory, CodingChallenge.DataLayer.ObjectFactory.ObjectDataFactory>();
            builder.Services.AddScoped<CodingChallenge.Business.Interfaces.ICityBusinessProvider, CodingChallenge.Business.CityBusinessProvider>();

            var maxBodySizeToCache = Convert.ToInt32(config["Values:MaxBodySizeToCache"]);
            //Enabling response caching
            builder.Services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = maxBodySizeToCache;
                options.UseCaseSensitivePaths = false;
            });


            var app = builder.Build();

            //If App is Running in Debug Mode
            bool isDebugMode = false;
#if DEBUG
            isDebugMode = true;
#endif
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || isDebugMode)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseResponseCaching();

            var cacheDurationInSeconds = Convert.ToInt32(config["Values:CacheResponseDurationInSeconds"]);

            //Register middleware to handle and log errors
            app.UseMiddleware<ExceptionHandlingMiddleware>();


            //Add Middleware - Response caching in the client - Ditching below as we cannot handle Vary by params- this works for static resources
           /* app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(cacheDurationInSeconds),
                        MustRevalidate = true,
                    };

                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });
            */


            app.MapControllers();

            app.Run();
        }
    }
}