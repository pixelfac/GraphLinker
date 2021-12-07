using System.Text.Json.Serialization;

namespace Models
{
    class TwitterUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        [JsonPropertyName("public_metrics")]
        public PublicMetrics PublicMetrics { get; set; }
    }

    class PublicMetrics
    {
        [JsonPropertyName("followers_count")]
        public int Followers { get; set; }
        
        [JsonPropertyName("following_count")]
        public int Following { get; set; }
        
        [JsonPropertyName("tweet_count")]
        public int Tweets { get; set; }
        
        [JsonPropertyName("listed_count")]
        public int Listed { get; set; }
    }
}