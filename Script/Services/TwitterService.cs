using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
            _client.BaseAddress = new Uri("https://api.twitter.com/2/");

            // Configure jsonOptions during request to ignore Upper/Lowercase
            if ( _jsonOptions is null )
            {
                _jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
            }
        }

        public async Task<TwitterUser> GetUserByHandle(string handle)
        {
            var response = await _client.GetAsync($"users/by/username/{handle}?user.fields=public_metrics,verified");

            PrintRateLimit(response.Headers, "GetUserByHandle Ratelimit: ");

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                await WaitForRateLimit(response.Headers);

                response = await _client.GetAsync($"users/by/username/{handle}?user.fields=public_metrics,verified");
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<UserResponse>(stream, _jsonOptions);

            return result.data;
        }

        public async Task<TwitterUser> GetUserById(string id)
        {
            var response = await _client.GetAsync($"users/{id}?user.fields=public_metrics,verified");
            
            PrintRateLimit(response.Headers, "GetUserById Ratelimit: ");

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                await WaitForRateLimit(response.Headers);

                response = await _client.GetAsync($"users/{id}?user.fields=public_metrics,verified");
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<UserResponse>(stream, _jsonOptions);

            return result.data;
        }

        public async Task<FollowingResponse> GetFollowingByUser(TwitterUser user, string pageToken = "")
        {
            Console.WriteLine($"Getting Following info for {user.Username} with page {pageToken}");
            
            HttpResponseMessage response;
            if (pageToken.Length > 1)
                response = await _client.GetAsync($"users/{user.Id}/following?max_results=1000&user.fields=public_metrics,verified&pagination_token={pageToken}");
            else
                response = await _client.GetAsync($"users/{user.Id}/following?max_results=1000&user.fields=public_metrics,verified");

            PrintRateLimit(response.Headers, "GetFollowingByUser Ratelimit: ");
            
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                await WaitForRateLimit(response.Headers);

                if (pageToken.Length > 1)
                    response = await _client.GetAsync($"users/{user.Id}/following?max_results=1000&user.fields=public_metrics,verified&pagination_token={pageToken}");
                else
                    response = await _client.GetAsync($"users/{user.Id}/following?max_results=1000&user.fields=public_metrics,verified");
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<FollowingResponse>(stream, _jsonOptions);

            if (result.meta.NextToken is not null)
            {
                var moreResult = await GetFollowingByUser(user, result.meta.NextToken);

                result.data = result.data.Concat(moreResult.data);
            }

            return result;
        }

        private async Task WaitForRateLimit(HttpResponseHeaders headers)
        {
            var resetLimit = headers.First(x => x.Key == "x-rate-limit-reset").Value.ElementAt(0);

            var futureTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(resetLimit));
            var time = futureTime - DateTime.UtcNow;

            while (time.Milliseconds > 0)
            {
                Console.WriteLine($"We are currently Rate Limited!\nWaiting for {time.Minutes + 1} minutes...");
                
                await Task.Delay(60000);

                resetLimit = headers.First(x => x.Key == "x-rate-limit-reset").Value.ElementAt(0);

                futureTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(resetLimit));
                time = futureTime - DateTime.UtcNow;
            }
        }

        private void PrintRateLimit(HttpResponseHeaders headers, string type)
        {
            var limit = headers.First(x => x.Key == "x-rate-limit-limit").Value.ElementAt(0);
            var remaining = headers.First(x => x.Key == "x-rate-limit-remaining").Value.ElementAt(0);
            var reset = headers.First(x => x.Key == "x-rate-limit-reset").Value.ElementAt(0);
            Console.WriteLine(String.Join(", ", type + limit, remaining, reset));
        }
    }
}