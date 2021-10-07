using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using NClient;
using NClient.Abstractions;
using NClient.Abstractions.Resilience;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
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
            IAsyncPolicy<IResponseContext<HttpRequestMessage, HttpResponseMessage>>? resiliencePolicy = null, 
            ILoggerFactory? loggerFactory = null)
        {
            var nclientFactoryBuilder = NClientGallery.NativeClientFactories
                .GetCustom()
                .For(factoryName: Guid.NewGuid().ToString())
                .UsingSystemHttpClient(httpClientFactory, httpClientName)
                .UsingSystemJsonSerializer()
                .WithIdempotentResilience();
            
            if (resiliencePolicy is not null)
                nclientFactoryBuilder.WithForcePollyResilience(resiliencePolicy);
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