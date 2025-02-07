using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using BinanceHistoricalData.Interfaces;
using BinanceHistoricalData.Repositories;
using BinanceHistoricalData.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BinanceHistoricalData.DependencyInjection
{
    public static class ExtensionServices
    {
        public static IServiceCollection AddBinanceHistoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(s =>
            {
                return new MongoClient(configuration.GetConnectionString("MongoDb"));
            });

            services.AddScoped<IBinanceRestClient, BinanceRestClient>();
            services.AddScoped<IBinanceService, BinanceService>();
            services.AddScoped<IHistoricalDataService, HistoricalDataService>();
            services.AddScoped<IHistoricalDataRepository, HistoricalDataRepository>();
            services.AddScoped<IJobsRepository, JobsRepository>();

            return services;
        }
    }
}
