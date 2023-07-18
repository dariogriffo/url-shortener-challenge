using System.ComponentModel.DataAnnotations;

namespace Url_Shortener.Models.Entities
{
    public class Url
    {
        [Key]
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
