using Url_Shortener.Models.Dtos.Request;
using Url_Shortener.Models.Dtos.Response;

namespace Url_Shortener.Services.Interfaces
{
    public interface IUrlService
    {
        Task Create(CreateUrlRequest url);
        Task Delete(string urlId);
        Task<UrlResponse> Get(string url);
        Task Update(string urlId);
        Task<IEnumerable<UrlResponse>> GetAll();
    }
}
