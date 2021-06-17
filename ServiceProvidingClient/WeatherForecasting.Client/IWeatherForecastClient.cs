using NClient.Abstractions;
using WeatherForecasting.Facade;

namespace WeatherForecasting.Client
{
    public interface IWeatherForecastClient : IWeatherForecastFacade, INClient
    {
    }
}