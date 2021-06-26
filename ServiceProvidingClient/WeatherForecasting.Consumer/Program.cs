using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient;
using WeatherForecasting.Client;

namespace WeatherForecasting.Consumer
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            var weatherForecastClientFactory = serviceProvider.GetRequiredService<IWeatherForecastClientFactory>();
            var weatherForecastClient = weatherForecastClientFactory.Create();

            var weatherForecasts = await weatherForecastClient.AsHttp().GetHttpResponse(x => x.GetAsync());
            foreach (var weatherForecast in weatherForecasts.Value!)
            {
                Console.WriteLine($"{weatherForecast.Date}: {weatherForecast.Summary}, {weatherForecast.TemperatureC} ℃");
            }
        }
        
        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient(nameof(IWeatherForecastClient));
            serviceCollection.AddLogging(x => x.AddConsole());
            serviceCollection.AddScoped<IWeatherForecastClientFactory>(x =>
            {
                var httpClientFactory = x.GetRequiredService<IHttpClientFactory>();
                return new WeatherForecastClientFactory(httpClientFactory, nameof(IWeatherForecastClient));
            });
            return serviceCollection.BuildServiceProvider();
        }
    }
}