# Quantc.NewsStories.WebAPI
Retrieves first n "best stories" from Hacker News API, sorted by their score


**How to run the application**
1. Clone to local repo
2. Make sure you have .NET 7 installed
3. Build the solution and start the Quantc.StoriesWebAPI project
4. Or, run the following command in the terminal
   
   `dotnet run --project Quantc.NewsStories.WebAPI.csproj`

**Assumptions**
- The requirement is to fetch _Stories_ only. Thus the naming convention focuses on _Stories_.
  This feature might be a part of a bigger picture and the naming would need to reflect it but it is unknown at this time.
- All external packages are at my disposal. Thus I am using Newtonsoft to handle Json.
- Tests were not the aim of this excercise. Otherwise StoryService would need coverage.

**Notes**
 - Two ways of caching are implemented
   - ResponseCaching for the requests with the same 'count' parameter. Set to 10min.
	- each call to HackerNews API is cached in MemoryCache. Not bulletproof since this cache is limited.
		 Set to 10min for inactive data and to absolute 1h.
 - Alternatively could go without Newtonsoft package but that requires implementing Unix Converter ourselves
			
**Given more time, I would think about**
 - Using OutputCaching from .NET 7	
 - Adding Rate limiting middleware e.g. TokenBucketRateLimiter in case current caching wasn't enough.
	 Fot the current scenario it is but fetching more often might require limiting API requests to HackerNews
 - Moving IMemoryCache setup away from the StoryService
 - Using e.g. Automapper in case more attributes needed custom converting logic
 - Adding logging & keeping statistics on this API usage (AppInsights)
 

