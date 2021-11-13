using System.Threading.Tasks;
using GithubRepoSummaryFetcher.Cli.GithubClient.Models;
using NClient.Annotations.Http;
using NClient.Providers.Results.HttpResults;

namespace GithubRepoSummaryFetcher.Cli.GithubClient
{
    [Header(name: "User-Agent", value: "GithubRepoSummaryFetcher.Cli")]
    public interface IGithubRepositoryClient
    {
        [GetMethod("users/{userName}/repos")]
        Task<HttpResponseWithError<RepositorySummary[], Error>> GetUserRepositoriesAsync([RouteParam] string userName);
        [GetMethod("orgs/{orgName}/repos")]
        Task<HttpResponseWithError<RepositorySummary[], Error>> GetOrgRepositoriesAsync([RouteParam] string orgName);
    }
}