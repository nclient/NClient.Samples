using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Providers.Results.HttpResults;
using WeatherForecasting.Facade;
using WeatherForecasting.Facade.Models;

namespace WeatherForecasting.Client
{
    public interface IWeatherForecastClient : IWeatherForecastFacade
    {
        [Override] 
        new Task<IHttpResponse<WeatherForecast[]>> GetAsync();
    }
}