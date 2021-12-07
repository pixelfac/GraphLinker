using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

using Models;

namespace Sercices
{
    class TwitterService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;
        public TwitterService(string token)
        {
            // Initialize Http Client
            _client = new HttpClient();
            
            // Get Headers to add Twitter Authentication
            var header = _client.DefaultRequestHeaders;

            // Get Bearer Token from Environment Variable and Add to header
            //string token = System.Environment.GetEnvironmentVariable("AUTH_TOKEN");
            header.Add("Authorization", $"Bearer {token}");
            
            // Set the Http Base address to twitter.com
            _client.BaseAddress = new Uri("https://api.twitter.com");

            // Configure jsonOptions during request to ignore Upper/Lowercase
            if ( _jsonOptions is null )
            {
                _jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
            }
        }

        public void GetUserByHandle(string handle)
        {

        }

        public void GetUserById(string id)
        {

        }

        public void GetFollowersById(ulong id)
        {
            
        }

        public async Task<FollowingResponse> GetFollowingById(string id, string pageToken = "")
        {
            Console.WriteLine($"Getting Following info for {id} with page {pageToken}");
            
            HttpResponseMessage response;
            if (pageToken.Length > 1)
                response = await _client.GetAsync($"2/users/{id}/following?max_results=1000&user.fields=public_metrics,verified&pagination_token={pageToken}");
            else
                response = await _client.GetAsync($"2/users/{id}/following?max_results=1000&user.fields=public_metrics,verified");

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                // Handle Rate Limit
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<FollowingResponse>(stream, _jsonOptions);

            if (result.meta.NextToken is not null)
            {
                var moreResult = await GetFollowingById(id, result.meta.NextToken);

                Console.WriteLine(moreResult.data.Count());

                result.data = result.data.Concat(moreResult.data);
            }

            return result;
        }

        public void GetIdByHandle(string handle)
        {

        }
    }
}