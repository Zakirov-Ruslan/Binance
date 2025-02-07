using BinanceHistoricalData.DependencyInjection;
using BinanceHistoricalData.Endpoints;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Logging.AddConsole();

            builder.Services.AddOpenApi();

            // Adding binacne library services
            builder.Services.AddBinanceHistoryServices(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Adding binacne library endpoint
            app.MapBinanceHistoryEndpoints();

            app.Run();
        }
    }
}
