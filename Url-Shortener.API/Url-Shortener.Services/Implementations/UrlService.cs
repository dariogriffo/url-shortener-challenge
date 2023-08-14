using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Create a short <see cref="Url"/> from the <see cref="CreateUrlRequest"/> parameter
        /// </summary>
        /// <returns>A short string variant of <see cref="Url"/></returns>
        public async Task<UrlResponse> Create(CreateUrlRequest url, HttpRequest httpRequest)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int codeLength = 6;
            string shorturl = $"{httpRequest.Scheme}://{httpRequest.Host}/{GenerateCode(characters, codeLength)}";

            if (!IsValidUrl(url.Url, out Uri? uri))
                throw new ArgumentException(@$"Invalid URL! Failed to shorten URL: {url.Url}. 
                Url can only contain alphanumeric characters, underscores, and dashes.");

            Url newUrl = new Url
            {
                LongUrl = uri!.ToString(),
                ShortUrl = shorturl
            };

            var result = await _urlManager.AddAsync(newUrl);
            if (result is null)
                throw new ArgumentNullException($"Sorry, Unable to create your short url with: {url.Url}");

            return new UrlResponse
            {
                Url = shorturl,
            };
        }

        public Task Delete(string urlId)
        {
            throw new NotImplementedException();
        }

        public async Task<UrlResponse?> Get(string shorturl)
        {
            if (shorturl is null)
                throw new ArgumentNullException();

            var result = await _urlManager.GetSingleByAsync(x => x.ShortUrl == shorturl);

            if (result is null)
                return new UrlResponse();

            return new UrlResponse
            {
                Url = result.LongUrl
            };
        }

        public Task<IEnumerable<UrlResponse>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(string urlId)
        {
            throw new NotImplementedException();
        }

        private string GenerateCode(string chars, int length)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[Random.Shared.Next(x.Length)]).ToArray());
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
