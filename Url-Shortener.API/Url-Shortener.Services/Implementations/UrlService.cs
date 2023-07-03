using Url_Shortener.Data.Context;
using Url_Shortener.Models.Dtos.Request;
using Url_Shortener.Models.Dtos.Response;
using Url_Shortener.Models.Entities;
using Url_Shortener.Services.Interfaces;

namespace Url_Shortener.Services.Implementations
{
    public class UrlService : IUrlService
    {

        public async Task Create(CreateUrlRequest url)
        {
            if (!Uri.TryCreate(url.Url, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException($"Wrong url: {url.Url} was tried to shorten");
            }
            
            //return Task.CompletedTask;
        }

        public Task Delete(string urlId)
        {
            throw new NotImplementedException();
        }

        public Task<UrlResponse> Get(string url)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UrlResponse>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(string urlId)
        {
            throw new NotImplementedException();
        }
    }
}
