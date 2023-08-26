using System.ComponentModel.DataAnnotations;

namespace Url_Shortener.Models.Dtos.Request
{
    public class CreateUrlRequest
    {
        [Required]
        public string Url { get; set; }
    }
}
