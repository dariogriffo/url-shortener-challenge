namespace UrlShortener.Endpoints
{
    public interface IUrlService
    {
        Task<Url?> GetByFullUrlAsync(string fullUrl);
        Task<Url?> GetByTinyUrlAsync(string tinyUrl);
        Task<Url> CreateAsync(string fullUrl, string alias);
        Task UpdateAsync(Url url);
    }
}

