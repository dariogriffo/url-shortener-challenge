namespace UrlShortener.Endpoints.Models
{
    public record GetUrlResponse(string FullUrl, string TinyUrl, int RequestedCount)
	{
	}
}

