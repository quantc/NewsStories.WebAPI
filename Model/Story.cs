using Newtonsoft.Json;

namespace Quantc.NewsStories.WebAPI.Model
{
    public class Story
    {
        [JsonProperty("title")]        
        public string Title { get; set; }

        [JsonProperty("url")]
        public string? URI { get; set; }

        [JsonProperty("by")]
        public string? PostedBy { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("descendants")]
        public int CommentCount { get; set; }
    }
}
