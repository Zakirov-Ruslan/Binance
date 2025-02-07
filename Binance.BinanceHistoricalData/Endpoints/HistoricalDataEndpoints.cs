using BinanceHistoricalData.DTO;
using BinanceHistoricalData.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BinanceHistoricalData.Endpoints
{
    public static class HistoricalDataEndpoints
    {
        public static void MapBinanceHistoryEndpoints(this WebApplication app)
        {
            app.MapPost("/api/historical-data/load",
                async ([FromBody] LoadDataRequest loadDataRequest,
                IHistoricalDataService historicalDataService) =>
            {
                try
                {
                    var response = await historicalDataService.LoadDataAsync(loadDataRequest);

                    var statusUrl = $"/api/historical-data/status?jobId={response.JobId}";

                    return Results.Created(statusUrl, response);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("LoadHistoicalData");

            app.MapGet("/api/historical-data/status",
                async ([FromQuery] string jobId,
                IHistoricalDataService historicalDataService) =>
            {
                try
                {
                    var status = await historicalDataService.GetJobStatusAsync(jobId);

                    return Results.Ok(status);
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException)
                        return Results.BadRequest(ex.Message);
                    else if (ex is KeyNotFoundException)
                        return Results.NotFound(ex.Message);
                    else
                        return Results.InternalServerError(ex.Message);
                }
            }).WithName("GetHistoricalDataStatus");
        }
    }
}
