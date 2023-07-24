namespace UrlShortener.Endpoints.Configuration
{
	public class UrlShortenerCacheSettings
    {
		public string SlidingExpirationUnit { get; init; }
		public int SlidingExpirationValue { get; init; }
		public string AbsoluteExpirationUnit { get; init; }
		public int AbsoluteExpirationValue { get; init; }
	}
}

