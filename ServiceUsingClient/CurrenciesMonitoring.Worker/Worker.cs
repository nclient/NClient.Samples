using System;
using System.Threading;
using System.Threading.Tasks;
using CurrenciesMonitoring.Worker.CoinbaseClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrenciesMonitoring.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ICoinbaseCurrenciesClient _coinbaseCurrenciesClient;
        private readonly ILogger<Worker> _logger;

        public Worker(ICoinbaseCurrenciesClient coinbaseCurrenciesClient, ILogger<Worker> logger)
        {
            _coinbaseCurrenciesClient = coinbaseCurrenciesClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                var currenciesResponse = await _coinbaseCurrenciesClient.Get().ConfigureAwait(false);
                _logger.LogInformation(
                    "Information about currencies is received. Number of currencies: {currenciesCount}.", 
                    currenciesResponse.Data.Length);
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}