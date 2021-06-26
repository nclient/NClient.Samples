using System.Net.Http;
using Microsoft.Extensions.Logging;
using NClient;
using NClient.Abstractions;
using NClient.Abstractions.Resilience;
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
            IAsyncPolicy<ResponseContext>? resiliencePolicy = null, 
            ILoggerFactory? loggerFactory = null)
        {
            var nclientFactoryBuilder = new NClientFactoryBuilder()
                .WithCustomHttpClient(httpClientFactory, httpClientName)
                .WithResiliencePolicyForIdempotentMethods();
            if (resiliencePolicy is not null)
                nclientFactoryBuilder.WithResiliencePolicy(resiliencePolicy);
            if (loggerFactory is not null)
                nclientFactoryBuilder.WithLogging(loggerFactory);
            
            _clientFactory = nclientFactoryBuilder.Build();
        }

        public IWeatherForecastClient Create()
        {
            return _clientFactory.Create<IWeatherForecastClient>(host: "http://localhost:5000");
        }
    }
}