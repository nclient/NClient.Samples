using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using WeatherForecasting.Facade.Models;

namespace WeatherForecasting.Facade
{
    [Api]
    [Path("[controller]")]
    public interface IWeatherForecastFacade
    {
        [GetMethod]
        Task<WeatherForecast[]> GetAsync();
    }
}