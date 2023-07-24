using System.Web;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UrlShortener.Endpoints.Configuration;

namespace UrlShortener.Endpoints.Services
{
	public class UrlCacheService: IUrlCacheService
    {
        private const string UrlListCacheKey = "UrlList";

        private readonly MemoryCacheEntryOptions _cacheOptions;
        private readonly IMemoryCache _memoryCache;

        public UrlCacheService(IOptions<UrlShortenerCacheSettings> urlShortenerCacheSettingsOptions, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

            var urlShortenerCacheSettings = urlShortenerCacheSettingsOptions.Value ??
                throw new ArgumentNullException(nameof(urlShortenerCacheSettingsOptions));

            _cacheOptions = new MemoryCacheEntryOptions()
             .SetSlidingExpiration(DefineSlidingExpiration(urlShortenerCacheSettings))
             .SetAbsoluteExpiration(DefineAbsoluteExpiration(urlShortenerCacheSettings));
        }

        private static TimeSpan DefineSlidingExpiration(UrlShortenerCacheSettings urlShortenerCacheSettings)
        {
            return urlShortenerCacheSettings.SlidingExpirationUnit switch
            {
                "Seconds" => TimeSpan.FromMinutes(urlShortenerCacheSettings.SlidingExpirationValue),
                "Minutes" => TimeSpan.FromMinutes(urlShortenerCacheSettings.SlidingExpirationValue),
                _ => TimeSpan.FromHours(1),
            };
        }

        private static TimeSpan DefineAbsoluteExpiration(UrlShortenerCacheSettings urlShortenerCacheSettings)
        {
            return urlShortenerCacheSettings.AbsoluteExpirationUnit switch
            {
                "Seconds" => TimeSpan.FromMinutes(urlShortenerCacheSettings.AbsoluteExpirationValue),
                "Minutes" => TimeSpan.FromMinutes(urlShortenerCacheSettings.AbsoluteExpirationValue),
                _ => TimeSpan.FromHours(1),
            };
        }

        public Url? Get(Predicate<Url> predicate)
        {
            var cachedUrls = _memoryCache.Get(UrlListCacheKey) as List<Url> ?? new List<Url>();
            var cachedUrl = cachedUrls?.SingleOrDefault(u => predicate(u));
            if (cachedUrl is not null)
            {
                return cachedUrl;
            }

            return null;
        }

        public void CreateOrUpdate(Url url)
        {
            var cachedUrls = _memoryCache.Get(UrlListCacheKey) as List<Url> ?? new List<Url>();
            var cachedUrl = cachedUrls.SingleOrDefault(u => u.FullUrl == url.FullUrl && u.TinyUrl == url.TinyUrl);
            if (cachedUrl is not null)
            {
                cachedUrl.Request();
            }
            else
            {
                cachedUrls.Add(url);
            }            
            _memoryCache.Set(UrlListCacheKey, cachedUrls, _cacheOptions);
        }
    }
}

