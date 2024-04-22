using Microsoft.AspNetCore.Mvc;
using Quantc.NewsStories.WebAPI.Model;
using Quantc.NewsStories.WebAPI.Services;

namespace Quantc.NewsStories.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly StoryService _storyService;

        public StoryController(StoryService storyService)
        {
            _storyService = storyService;
        }

        [MapToApiVersion("1")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any,
            VaryByQueryKeys = new string[] { "count" })]
        [HttpGet]
        [Route("best")]
        public async Task<ActionResult<IEnumerable<Story>>> Get(int count = 3)
        {
            var result = await _storyService.GetBestStoriesAsync(count);

            return Ok(result);
        }
    }
}