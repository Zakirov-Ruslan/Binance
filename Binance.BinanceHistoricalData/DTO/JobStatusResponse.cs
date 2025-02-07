namespace BinanceHistoricalData.DTO
{
    public record JobStatusResponse
    (
        string JobId,
        string Status,
        DateTime? EndTime 
    );
}
