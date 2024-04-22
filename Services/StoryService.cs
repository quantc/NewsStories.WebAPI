using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quantc.NewsStories.WebAPI.Common;
using Quantc.NewsStories.WebAPI.Model;
using System.Collections.Concurrent;

namespace Quantc.NewsStories.WebAPI.Services
{
    public class StoryService
    {
        private const string CacheKey = "NewsStories";
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public StoryService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }
       
        public async Task<IEnumerable<Story>?> GetBestStoriesAsync(int numberOfStories)
        {
            _cache.TryGetValue(CacheKey, out IEnumerable<Story>? cachedStories);

            if (cachedStories?.Count() >= numberOfStories)
            {
                return cachedStories.Take(numberOfStories).ToList();
            }

            var storiesIds = await _httpClient.GetFromJsonAsync<IEnumerable<int>>(
                UriSpace.BestStories);

            ConcurrentBag<Story> stories = new();
            if (storiesIds == null)
            {
                return null;
            }

            await Parallel.ForEachAsync(storiesIds, async (storyId, cancellationToken) =>
            {
                var response = await _httpClient.GetAsync(string.Format(UriSpace.SingleStory, storyId), cancellationToken);
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                response.EnsureSuccessStatusCode();

                if (responseContent != null)
                {
                    var story = JsonConvert.DeserializeObject<Story>(responseContent, new UnixDateTimeConverter());
                    stories.Add(story);
                }
            });

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromSeconds(600))
                       .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                       .SetPriority(CacheItemPriority.Normal)
                       .SetSize(1024);
            _cache.Set(CacheKey, stories, cacheEntryOptions);

            var ordered = stories.OrderByDescending(x => x.Score).Take(numberOfStories).ToList();
            return ordered;
        }
    }
}
