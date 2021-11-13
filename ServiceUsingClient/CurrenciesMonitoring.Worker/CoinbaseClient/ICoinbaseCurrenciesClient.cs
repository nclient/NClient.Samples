using System.Threading.Tasks;
using CurrenciesMonitoring.Worker.CoinbaseClient.Models;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace CurrenciesMonitoring.Worker.CoinbaseClient
{
    [Path("v2/currencies")]
    public interface ICoinbaseCurrenciesClient
    {
        [GetMethod]
        Task<CurrenciesResponse> Get();
    }
}