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

        [HttpPost("create-short-url", Name = "Create new short url")]
        public async Task<IActionResult> Create([FromBody] CreateUrlRequest url)
        {
            var response = await _urlService.Create(url, HttpContext);
            return Ok(response);
        }

        [HttpGet("get-full-url", Name = "Get complete url")]
        public async Task<IActionResult> Get(string url)
        {
            //var path = HttpContext.Request.Path.ToUriComponent().Trim('/');
            var response = await _urlService.Get(url);
            return Ok(response);
        }
    }
}
