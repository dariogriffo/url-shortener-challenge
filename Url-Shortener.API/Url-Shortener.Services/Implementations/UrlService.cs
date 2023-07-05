using System.Text.RegularExpressions;
using Url_Shortener.Data.Repository.Interfaces;
using Url_Shortener.Models.Dtos.Request;
using Url_Shortener.Models.Dtos.Response;
using Url_Shortener.Models.Entities;
using Url_Shortener.Services.Interfaces;

namespace Url_Shortener.Services.Implementations
{
    public class UrlService : IUrlService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Url> _urlManager;

        public UrlService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _urlManager = _unitOfWork.GetRepository<Url>();
        }

        private static readonly Regex WebsiteLinkRegex = new Regex(
            @"^(https?://)?([a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+(/[a-zA-Z0-9-./?%&=]*)?)?$",
            RegexOptions.None,
            TimeSpan.FromMilliseconds(1)
        );

        public async Task<Url> Create(CreateUrlRequest url)
        {
            if (!IsValidUrl(url.Url, out Uri? uri))
                throw new ArgumentException(@$"Invalid URL! Failed to shorten URL: {url.Url}. 
                Url can only contain alphanumeric characters, underscores, and dashes.");

            Url newUrl = new Url
            {
                Id = Guid.NewGuid().ToString(),
                LongUrl = uri.ToString(),
                ShortUrl = url.Url,
            };

            var result = await _urlManager.AddAsync(newUrl);
            if (result is null)
                throw new ArgumentNullException($"Sorry, Unable to create your short url with: {url.Url}");

            return result;
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

        private bool IsValidUrl(string url, out Uri? absoluteUri)
        {
            if (!WebsiteLinkRegex.IsMatch(url))
            {
                absoluteUri = null;
                return false;
            }
                
            if (!Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                string baseUrl = "https://";
                url = $"{baseUrl}{url}";
                absoluteUri = new Uri(url);

                if (absoluteUri is null)
                    return false;
            }

            return true;
        }
    }
}
