using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using WeatherForecasting.Facade.Models;

namespace WeatherForecasting.Facade
{
    [HttpFacade, Path("[controller]")]
    public interface IWeatherForecastFacade
    {
        [GetMethod]
        Task<WeatherForecast[]> GetAsync();
    }
}