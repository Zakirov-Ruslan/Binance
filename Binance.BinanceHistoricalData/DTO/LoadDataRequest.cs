namespace BinanceHistoricalData.DTO
{
    public record LoadDataRequest
    (
        string[] Pairs,
        DateTime StartDate,
        DateTime EndDate
    );
}
