using CodingChallenge.Logging;
using CodingChallenge.Logging.Interface;

namespace CodingChallengeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Console.WriteLine("Fetching Environment Variable ASPNETCORE_ENVIRONMENT =" + env);

            var appSettingFileName = string.Join(".", "appsettings", env, "json");
            Console.WriteLine("AppSetting FileName =" + appSettingFileName);


            

            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile(appSettingFileName).Build();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddResponseCaching();//For adding related dependencies

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var _logger = new LoggerFactory().CreateLogger("CustomCategory");
            
            //Adding Custom middlewre to handle the logs
            builder.Services.AddScoped<CodingChallenge.Logging.Interface.ILogging, CodingChallenge.Logging.Logger>(_logger);

            var maxBodySizeToCache = Convert.ToInt32(config["Values:MaxBodySizeToCache"]);
            //Enabling response caching
            builder.Services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = maxBodySizeToCache;
                options.UseCaseSensitivePaths = false;
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseResponseCaching();

            var cacheDurationInSeconds = Convert.ToInt32(config["Values:CacheResponseDurationInSeconds"]);

            //Add Middleware
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(cacheDurationInSeconds),

                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.MapControllers();

            app.Run();
        }
    }
}