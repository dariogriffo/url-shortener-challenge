using System.ComponentModel.DataAnnotations;

namespace Url_Shortener.Models.Dtos.Request
{
    public class UrlRequest
    {
        [Required]
        public string Url { get; set; }
    }
}
