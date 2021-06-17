using System;
using System.Text.Json.Serialization;

#nullable disable

namespace GithubRepoSummaryFetcher.Cli.GithubClient.Models
{
    public class RepositorySummary
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
        
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }
    }
}