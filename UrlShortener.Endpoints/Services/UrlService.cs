using System.Web;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UrlShortener.Endpoints.Helpers;
using UrlShortener.Endpoints.Services;

namespace UrlShortener.Endpoints
{
    public class UrlService: IUrlService
    {
        private readonly IMongoCollection<Url> _urls;
        private readonly IUrlCacheService _urlCacheService;

        public UrlService(IOptions<UrlShortenerDatabaseSettings> urlShortenerDatabaseSettingsOptions, IUrlCacheService urlCacheService)
        {
            _urlCacheService = urlCacheService;
            var urlShortenerDatabaseSettings = urlShortenerDatabaseSettingsOptions.Value ??
                throw new ArgumentNullException(nameof(urlShortenerDatabaseSettingsOptions));

            var client = new MongoClient(urlShortenerDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(urlShortenerDatabaseSettings.DatabaseName);
            _urls = database.GetCollection<Url>(urlShortenerDatabaseSettings.CollectionName);
        }

        public async Task<Url> CreateAsync(string fullUrl, string alias)
        {
            var url = _urlCacheService.Get(u => u.FullUrl.Equals(HttpUtility.UrlDecode(fullUrl), StringComparison.OrdinalIgnoreCase));
            if (url is null)
            {
                var filter = Builders<Url>.Filter.Where(e => e.FullUrl.Equals(fullUrl, StringComparison.OrdinalIgnoreCase));
                url = (await _urls.FindAsync<Url>(filter)).FirstOrDefault();

                if (url is null)
                {
                    url = Url.Create(fullUrl, alias);
                    var tinyUrl = fullUrl.CreateShortUrl(alias);
                    if (await GetByTinyUrlAsync(tinyUrl) is not null)
                    {
                        url.SetTinyUrl(fullUrl.CreateShortUrl(string.Empty));
                    }
                    else
                    {
                        url.SetTinyUrl(tinyUrl);
                    }
                    await _urls.InsertOneAsync(url);
                }
            }

            _urlCacheService.CreateOrUpdate(url);
            return url;
        }

        public async Task<Url?> GetByFullUrlAsync(string fullUrl)
        {
            var url = _urlCacheService.Get(u => u.FullUrl.Equals(HttpUtility.UrlDecode(fullUrl), StringComparison.OrdinalIgnoreCase));
            if (url is null)
            {
                var filter = Builders<Url>.Filter.Where(e => e.FullUrl.Equals(HttpUtility.UrlDecode(fullUrl), StringComparison.OrdinalIgnoreCase));
                url = (await _urls.FindAsync<Url>(filter)).FirstOrDefault();
            }

            _urlCacheService.CreateOrUpdate(url);
            return url;
        }

        public async Task<Url?> GetByTinyUrlAsync(string tinyUrl)
        {
            var url = _urlCacheService.Get(u => u.TinyUrl.Equals(HttpUtility.UrlDecode(tinyUrl), StringComparison.OrdinalIgnoreCase));
            if (url is null)
            {
                var filter = Builders<Url>.Filter.Where(e => e.TinyUrl.Equals(HttpUtility.UrlDecode(tinyUrl), StringComparison.OrdinalIgnoreCase));
                url = (await _urls.FindAsync<Url>(filter)).FirstOrDefault();
            }

            _urlCacheService.CreateOrUpdate(url);
            return url;
        }

        public async Task UpdateAsync(Url url)
        {
            var filter = Builders<Url>.Filter.Where(e => e.TinyUrl.Equals(HttpUtility.UrlDecode(url.TinyUrl), StringComparison.OrdinalIgnoreCase) &&
                                                         e.FullUrl.Equals(HttpUtility.UrlDecode(url.FullUrl), StringComparison.OrdinalIgnoreCase));
            var update = Builders<Url>.Update.Set(u => u.RequestedCount, url.RequestedCount + 1);
            await _urls.UpdateOneAsync(filter, update);
        }
    }
}

