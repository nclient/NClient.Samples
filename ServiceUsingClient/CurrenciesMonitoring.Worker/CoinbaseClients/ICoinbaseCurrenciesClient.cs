using System.Threading.Tasks;
using CurrenciesMonitoring.Worker.CoinbaseClients.Models;
using NClient.Annotations;
using NClient.Annotations.Methods;

namespace CurrenciesMonitoring.Worker.CoinbaseClients
{
    [Path("v2/currencies")]
    public interface ICoinbaseCurrenciesClient
    {
        [GetMethod]
        Task<CurrenciesResponse> Get();
    }
}