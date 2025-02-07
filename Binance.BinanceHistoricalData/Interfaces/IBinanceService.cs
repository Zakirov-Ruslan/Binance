using Binance.Net.Interfaces;

namespace BinanceHistoricalData.Interfaces
{
    public interface IBinanceService
    {
        Task<IEnumerable<IBinanceKline>> GetHistoricalTrades(string symbol, DateTime startTime, DateTime endTime);
    }
}