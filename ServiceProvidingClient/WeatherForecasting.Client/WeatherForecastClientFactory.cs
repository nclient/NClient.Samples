using System.Net.Http;
using Microsoft.Extensions.Logging;
using NClient;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using Polly;

namespace WeatherForecasting.Client
{
    public interface IWeatherForecastClientFactory
    {
        IWeatherForecastClient Create();
    }

    public class WeatherForecastClientFactory : IWeatherForecastClientFactory
    {
        private readonly INClientFactory _clientFactory;
        
        public WeatherForecastClientFactory(
            IHttpClientFactory httpClientFactory,
            string? httpClientName = null,
            IAsyncPolicy<HttpResponse>? resiliencePolicy = null, 
            ILoggerFactory? loggerFactory = null)
        {
            _clientFactory = new NClientFactory(
                httpClientFactory,
                httpClientName,
                jsonSerializerOptions: null,
                resiliencePolicy,
                loggerFactory);
        }

        public IWeatherForecastClient Create()
        {
            return _clientFactory.Create<IWeatherForecastClient>(host: "http://localhost:5000");
        }
    }
}