using BinanceHistoricalData.Interfaces;
using BinanceHistoricalData.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BinanceHistoricalData.Repositories
{
    public class JobsRepository : IJobsRepository
    {
        private readonly IMongoClient _mongoClient;
        private readonly ILogger<IJobsRepository> _logger;

        public JobsRepository(IMongoClient mongoClient, ILogger<IJobsRepository> logger)
        {
            _mongoClient = mongoClient;
            _logger = logger;
        }

        public async Task<DataLoadJob> GetDataLoadJobAsync(ObjectId id)
        {
            // Getting database and collection
            var db = _mongoClient.GetDatabase("binance");
            var jobsCollection = db.GetCollection<DataLoadJob>("jobs");

            // Searching for a job by filter
            var filter = Builders<DataLoadJob>.Filter.Eq(job => job.Id, id);
            var result = await jobsCollection.FindAsync(filter);

            var dataLoadJob = result?.FirstOrDefault();

            // Return if found, else throwing exception
            if (dataLoadJob == null)
                throw new KeyNotFoundException($"No tasks found with id {id}.");

            return dataLoadJob;
        }

        public async Task<DataLoadJob> AddJob(DataLoadJob dataLoadJob)
        {
            if (dataLoadJob == null)
                throw new ArgumentNullException("DataLoadJob cannot be null");

            // Getting database and collection
            var db = _mongoClient.GetDatabase("binance");
            var jobsCollection = db.GetCollection<DataLoadJob>("jobs");

            // Adding job to database and returning it
            await jobsCollection.InsertOneAsync(dataLoadJob);

            _logger.LogInformation($"Job with id {dataLoadJob.Id} added.");

            return dataLoadJob;
        }

        public async Task UpdateJobStatus(DataLoadJob dataLoadJob)
        {
            if (dataLoadJob == null)
                throw new ArgumentNullException("DataLoadJob cannot be null");

            // Getting database and collection
            var db = _mongoClient.GetDatabase("binance");
            var jobsCollection = db.GetCollection<DataLoadJob>("jobs");

            // Update job status property by id filter
            var filter = Builders<DataLoadJob>.Filter.Eq(job => job.Id, dataLoadJob.Id);
            var update = Builders<DataLoadJob>.Update.Set(job => job.Status, dataLoadJob.Status);

            await jobsCollection.UpdateOneAsync(filter, update);

            _logger.LogInformation($"Job with id {dataLoadJob.Id} seted status {dataLoadJob.Status}.");
        }

        public async Task EndJob(DataLoadJob dataLoadJob)
        {
            if (dataLoadJob == null)
                throw new ArgumentNullException("DataLoadJob cannot be null");

            // Getting database and collection
            var db = _mongoClient.GetDatabase("binance");
            var jobsCollection = db.GetCollection<DataLoadJob>("jobs");

            // Setting end time and ended status using id filter
            var filter = Builders<DataLoadJob>.Filter.Eq(job => job.Id, dataLoadJob.Id);
            var update = Builders<DataLoadJob>.Update.
                Set(job => job.Status, dataLoadJob.Status).
                Set(job => job.EndTime, dataLoadJob.EndTime);

            await jobsCollection.UpdateOneAsync(filter, update);

            // Logging metrics
            var timeSpend = (dataLoadJob.EndTime.Value - dataLoadJob.StartTime).TotalSeconds;
            _logger.LogInformation($"Job with id {dataLoadJob.Id} successfully ended.  Time spend {timeSpend} secs.");
        }
    }
}
