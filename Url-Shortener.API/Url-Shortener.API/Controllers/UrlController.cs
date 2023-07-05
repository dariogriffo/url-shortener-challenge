using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Url_Shortener.Models.Dtos.Request;
using Url_Shortener.Services.Interfaces;

namespace Url_Shortener.API.Controllers
{
    [Route("api/url")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        public readonly IUrlService _urlService;
        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost(Name = "Create short url")]
        public async Task<IActionResult> Create([FromBody] CreateUrlRequest url)
        {
            var response = await _urlService.Create(url);
            return Ok(response);
        }
    }
}
