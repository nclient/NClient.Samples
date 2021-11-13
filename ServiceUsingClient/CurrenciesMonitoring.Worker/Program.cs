using CurrenciesMonitoring.Worker.CoinbaseClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                    services
                        .AddHostedService<Worker>()
                        .AddLogging();

                    services
                        .AddNClient<ICoinbaseCurrenciesClient>(
                            host: "https://api.coinbase.com",
                            implementationFactory: (x, builder) => builder
                                .WithFullResilience()
                                .WithLogging(x.GetRequiredService<ILogger<ICoinbaseCurrenciesClient>>())
                                .Build());
                });
        }
    }
}