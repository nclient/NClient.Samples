using CurrenciesMonitoring.Worker.CoinbaseClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NClient;
using NClient.Extensions.DependencyInjection;

namespace CurrenciesMonitoring.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>()
                        .AddLogging()
                        .AddHttpClient(nameof(ICoinbaseCurrenciesClient));
                    
                    services.AddNClient<ICoinbaseCurrenciesClient>(
                        host: "https://api.coinbase.com", 
                        configure: x => x.WithResiliencePolicy(retryCount: 2),
                        httpClientName: nameof(ICoinbaseCurrenciesClient));
                });
        }
    }
}