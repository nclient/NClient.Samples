using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using NClient;
using NClient.Providers.Api.Rest.Extensions;
using NClient.Providers.Transport;
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
            var nclientFactoryBuilder = NClientGallery.ClientFactories.GetCustom()
                .For(factoryName: Guid.NewGuid().ToString())
                .UsingRestApi()
                .UsingSystemHttpTransport(httpClientFactory, httpClientName)
                .UsingSystemJsonSerialization()
                .WithIdempotentResilience();
            
            if (resiliencePolicy is not null)
                nclientFactoryBuilder.WithPollyFullResilience(resiliencePolicy);
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