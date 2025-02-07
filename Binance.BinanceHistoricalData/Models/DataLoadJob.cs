using MongoDB.Bson;

namespace BinanceHistoricalData.Models
{
    public class DataLoadJob
    {
        public ObjectId Id { get; private set; }
        public string Status { get; private set; } = null!;
        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        public static DataLoadJob Create()
        {
            DataLoadJob job = new();
            job.Status = "В обработке";
            job.StartTime = DateTime.Now;

            return job;
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }

        public void End()
        {
            EndTime = DateTime.Now;
            Status = "Завершено";
        }
    }
}
