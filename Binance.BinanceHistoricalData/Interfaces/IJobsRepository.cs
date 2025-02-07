using BinanceHistoricalData.Models;
using MongoDB.Bson;

namespace BinanceHistoricalData.Interfaces
{
    public interface IJobsRepository
    {
        Task<DataLoadJob> GetDataLoadJobAsync(ObjectId id);
        Task<DataLoadJob> AddJob(DataLoadJob dataLoadJob);
        Task UpdateJobStatus(DataLoadJob dataLoadJob);
        Task EndJob(DataLoadJob dataLoadJob);
    }
}