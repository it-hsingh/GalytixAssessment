using GalytixAssessment.Data;
using Microsoft.OpenApi.Models;
using System.Data;

namespace GalytixAssessment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IDataLoader, CsvDataLoader>();
            builder.Services.AddSingleton<IGwpRepository, GwpRepository>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Galytix API",
                    Description = "An ASP.NET Core Web API for managing Galytix items",
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();


            app.MapControllers();

            // Load data at application startup
            var dataLoader = app.Services.GetRequiredService<IDataLoader>();
            var rawData = dataLoader.LoadData();

            var gwpRepo = app.Services.GetRequiredService<IGwpRepository>() as GwpRepository;
            gwpRepo?.SetData(rawData);

            app.Run("http://localhost:9091");
        }
    }
}
