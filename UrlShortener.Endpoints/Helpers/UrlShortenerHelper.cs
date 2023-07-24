namespace UrlShortener.Endpoints.Helpers
{
	public static class UrlShortenerHelper
    {
        private const string ShortenerBaseUrl = "https://sortener.sn";

        public static string CreateShortUrl(this string url, string alias)
		{
            return $"{ShortenerBaseUrl}/{string.Format("{0:X}", (string.IsNullOrEmpty(alias) ? url.GetHashCode() : alias))}";

        }
	}
}

