using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace WeatherApp
{
    public class GetWeatherApp
    {
        private readonly ILogger _logger;
        private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


        public GetWeatherApp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetWeatherApp>();
        }

        [Function("GetWeatherApp")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var weatherData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();           

            var strWeatherData ="";
            try
            {
                foreach (WeatherForecast w in weatherData)
                {
                    strWeatherData += w.ToString();
                }
            }
            catch (Exception)
            { }
            
            if (strWeatherData != null)
                response.WriteString(strWeatherData);

            return response;
        }
    }
}
