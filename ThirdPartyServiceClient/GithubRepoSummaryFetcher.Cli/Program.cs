using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GithubRepoSummaryFetcher.Cli.GithubClient;
using GithubRepoSummaryFetcher.Cli.GithubClient.Models;
using NClient;
using NClient.Providers.Transport;

namespace GithubRepoSummaryFetcher.Cli
{
    internal static class Program
    {
        private static readonly IGithubRepositoryClient GithubRepositoryClient;

        static Program()
        {
            bool IsSuccess(IResponseContext<HttpRequestMessage, HttpResponseMessage> context) => 
                context.Response.IsSuccessStatusCode || context.Response.StatusCode == HttpStatusCode.NotFound;
            
            GithubRepositoryClient = NClientGallery.Clients.GetRest()
                .For<IGithubRepositoryClient>(host: "https://api.github.com")
                .WithResponseValidation(IsSuccess, onFailure: _ => { })
                .WithIdempotentResilience(shouldRetry: context => !IsSuccess(context))
                .Build();
        }
        
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
                throw new AggregateException("The account name is not passed as an argument.");
            if (args.Length > 1)
                throw new AggregateException("The account name must be one.");

            var repositories = await GetRepositoriesAsync(args.Single());
            
            Console.WriteLine("Repositories:");
            foreach (var repository in repositories)
            {
                Console.WriteLine($"{repository.Name} - {repository.HtmlUrl}");
            }
        }

        private static async Task<RepositorySummary[]> GetRepositoriesAsync(string accountName)
        {
            var userRepositoriesResponse = await GithubRepositoryClient.GetUserRepositoriesAsync(accountName);
            if (userRepositoriesResponse.IsSuccessful)
                return userRepositoriesResponse.Data!;

            var orgRepositoriesResponse = await GithubRepositoryClient.GetOrgRepositoriesAsync(accountName);
            if (orgRepositoriesResponse.IsSuccessful)
                return orgRepositoriesResponse.Data!;

            if (userRepositoriesResponse.StatusCode == HttpStatusCode.NotFound && orgRepositoriesResponse.StatusCode == HttpStatusCode.NotFound)
                throw new Exception("Account not found.");
            throw new AggregateException(userRepositoriesResponse.ErrorException!, orgRepositoriesResponse.ErrorException!);
        }
    }
}