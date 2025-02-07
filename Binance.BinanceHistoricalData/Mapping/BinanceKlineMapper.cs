using Binance.Net.Interfaces;
using BinanceHistoricalData.Models;

namespace BinanceHistoricalData.Mapping
{
    public static class BinanceKlineMapper
    {
        public static BinanceKline ToStorageModel(this IBinanceKline kline)
        {
            if (kline == null)
            {
                throw new ArgumentNullException(nameof(kline), "Source IBinanceKline cannot be null.");
            }

            return new BinanceKline
            {
                OpenTime = kline.OpenTime,
                OpenPrice = kline.OpenPrice,
                HighPrice = kline.HighPrice,
                LowPrice = kline.LowPrice,
                ClosePrice = kline.ClosePrice,
                Volume = kline.Volume,
                CloseTime = kline.CloseTime,
                QuoteVolume = kline.QuoteVolume,
                TradeCount = kline.TradeCount,
                TakerBuyBaseVolume = kline.TakerBuyBaseVolume,
                TakerBuyQuoteVolume = kline.TakerBuyQuoteVolume
            };
        }
    }
}
