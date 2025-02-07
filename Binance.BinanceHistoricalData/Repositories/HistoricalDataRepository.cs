using BinanceHistoricalData.Interfaces;
using BinanceHistoricalData.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BinanceHistoricalData.Repositories
{
    public class HistoricalDataRepository : IHistoricalDataRepository
    {
        private readonly IMongoClient _mongoClient;
        private readonly ILogger<IHistoricalDataRepository> _logger;

        public HistoricalDataRepository(IMongoClient mongoClient, ILogger<IHistoricalDataRepository> logger)
        {
            _mongoClient = mongoClient;
            _logger = logger;
        }

        public async Task AddHistoricalData(IEnumerable<BinanceKline> klines)
        {
            if (klines == null || !klines.Any())
            {
                throw new ArgumentException("Klines collection cannot be null or empty.", nameof(klines));
            }

            // Getting database and collection
            var db = _mongoClient.GetDatabase("binance");
            var klinesCollection = db.GetCollection<BinanceKline>("klines");

            // Adding historical data
            await klinesCollection.InsertManyAsync(klines);

            _logger.LogInformation($"Inserted {klines.Count()} klines to database");
        }
    }
}