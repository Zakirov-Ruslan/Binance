using BinanceHistoricalData.Models;

namespace BinanceHistoricalData.Interfaces
{
    public interface IHistoricalDataRepository
    {
        Task AddHistoricalData(IEnumerable<BinanceKline> klines);
    }
}