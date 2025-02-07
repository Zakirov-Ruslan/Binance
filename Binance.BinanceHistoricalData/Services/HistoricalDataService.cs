using BinanceHistoricalData.DTO;
using BinanceHistoricalData.Interfaces;
using BinanceHistoricalData.Models;
using BinanceHistoricalData.Mapping;
using MongoDB.Bson;
using Binance.Net.Interfaces;

namespace BinanceHistoricalData.Services
{
    public class HistoricalDataService : IHistoricalDataService
    {
        private readonly IHistoricalDataRepository _historicalDataRepository;
        private readonly IJobsRepository _jobsRepository;
        private readonly IBinanceService _binanceService;

        public HistoricalDataService(IHistoricalDataRepository historicalDataRepository, IJobsRepository jobsRepository , IBinanceService binanceService)
        {
            _historicalDataRepository = historicalDataRepository;
            _jobsRepository = jobsRepository;
            _binanceService = binanceService;
        }

        public async Task<LoadDataResponse> LoadDataAsync(LoadDataRequest loadDataRequest)
        {
            // Creating job and writing it in db
            var dataLoadJob = DataLoadJob.Create();
            dataLoadJob = await _jobsRepository.AddJob(dataLoadJob);

            // Invoke data loading from binance api without awaiting
            _ = ExecuteDataLoadAsync(loadDataRequest, dataLoadJob);

            // Return created job id
            var response = new LoadDataResponse(dataLoadJob.Id.ToString());
            return response;
        }

        private async Task ExecuteDataLoadAsync(LoadDataRequest loadDataRequest, DataLoadJob dataLoadJob)
        {
            try
            {
                // Getting historical data from binance api service
                List<IBinanceKline> klines = new List<IBinanceKline>();
                foreach (var pair in loadDataRequest.Pairs)
                {
                    var pairKlines = await _binanceService.GetHistoricalTrades(pair, loadDataRequest.StartDate, loadDataRequest.EndDate);
                    klines.AddRange(pairKlines);
                }

                // Casting to db model and writing 
                var storageKlines = klines.Select(k => k.ToStorageModel());
                await _historicalDataRepository.AddHistoricalData(storageKlines);

                // Setting job status to ended and writing to db
                dataLoadJob.End();
                await _jobsRepository.EndJob(dataLoadJob);
            }
            catch (Exception)
            {
                // If proceed any exception - writing error status to db
                dataLoadJob.UpdateStatus("Ошибка");
                await _jobsRepository.UpdateJobStatus(dataLoadJob);
                throw;
            }
        }

        public async Task<JobStatusResponse> GetJobStatusAsync(string jobId)
        {
            // Getting job status from storage
            ObjectId objectId = ObjectId.Parse(jobId);
            var job = await _jobsRepository.GetDataLoadJobAsync(objectId);

            return new JobStatusResponse(job.Id.ToString(), job.Status, job.EndTime);
        }
    }
}
