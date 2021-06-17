using System;
using CurrenciesMonitoring.Worker.CoinbaseClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NClient;
using NClient.Abstractions.HttpClients;
using NClient.Extensions.DependencyInjection;
using Polly;

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
                    
                    var retryPolicy = Policy
                        .HandleResult<HttpResponse>(x => !x.IsSuccessful)
                        .Or<Exception>()
                        .WaitAndRetryAsync(retryCount: 2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                    services.AddNClient<ICoinbaseCurrenciesClient>(
                        host: "https://api.coinbase.com", 
                        configure: x => x.WithResiliencePolicy(retryPolicy),
                        httpClientName: nameof(ICoinbaseCurrenciesClient));
                });
        }
    }
}