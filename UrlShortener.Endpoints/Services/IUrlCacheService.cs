namespace UrlShortener.Endpoints.Services
{
    public interface IUrlCacheService
    {
        Url? Get(Predicate<Url> predicate);
        void CreateOrUpdate(Url url);
    }
}

