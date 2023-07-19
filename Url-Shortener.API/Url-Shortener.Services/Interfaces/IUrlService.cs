using Microsoft.AspNetCore.Http;
using Url_Shortener.Models.Dtos.Request;
using Url_Shortener.Models.Dtos.Response;
using Url_Shortener.Models.Entities;

namespace Url_Shortener.Services.Interfaces
{
    public interface IUrlService
    {
        Task<UrlResponse> Create(CreateUrlRequest url, HttpContext httpContext);
        Task Delete(string urlId);
        Task<UrlResponse> Get(string url);
        Task Update(string urlId);
        Task<IEnumerable<UrlResponse>> GetAll();
        int Decode(string path);
    }
}
