using BinanceHistoricalData.DTO;

namespace BinanceHistoricalData.Interfaces
{
    public interface IHistoricalDataService
    {
        Task<LoadDataResponse> LoadDataAsync(LoadDataRequest loadDataRequest);
        Task<JobStatusResponse> GetJobStatusAsync(string jobId);
    }
}