using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{
    class FollowingResponse
    {
        [JsonPropertyName("data")]
        public IEnumerable<TwitterUser> data { get; set;}

        [JsonPropertyName("meta")]
        public Meta meta { get; set; }
    }
    
    class Meta
    {
        [JsonPropertyName("result_count")]
        public int ResultCount { get; set; }

        [JsonPropertyName("next_token")]
        public string NextToken { get; set; }
    }
}