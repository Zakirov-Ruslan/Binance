using Binance.Net.Interfaces;
using Binance.Net.Interfaces.Clients;
using BinanceHistoricalData.Interfaces;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;

namespace BinanceHistoricalData.Services
{
    public class BinanceService : IBinanceService
    {
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly ILogger<IBinanceService> _logger;

        public BinanceService(IBinanceRestClient binanceRestClient, ILogger<IBinanceService> logger)
        {
            _binanceRestClient = binanceRestClient;
            _logger = logger;
        }

        public async Task<IEnumerable<IBinanceKline>> GetHistoricalTrades(string symbol, DateTime startTime, DateTime endTime)
        {
            var interval = Binance.Net.Enums.KlineInterval.OneHour;

            var result = await _binanceRestClient.SpotApi.ExchangeData.GetKlinesAsync(symbol, interval, startTime: startTime, endTime: endTime);

            if (result.Success)
            {
                IEnumerable<IBinanceKline> klines = result.Data;

                return klines;
            }
            else
            {
                // Error always is not null if !Success
                _logger.LogError(result.Error.Message);
                throw new Exception(result.Error.Message);
            }
        }
    }
}
