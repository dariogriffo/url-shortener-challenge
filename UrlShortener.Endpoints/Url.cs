using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortener.Endpoints
{
    public class Url
    {
        private Url(string url, string alias)
        {           
            FullUrl = url ?? throw new ArgumentNullException(nameof(url));
            Alias = alias;
        }

        [BsonId]
        [JsonIgnore]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        [Required]
        public string FullUrl { get; private set; }
        [Required]
        public string TinyUrl { get; private set; }
        public string Alias { get; private set; }
        public int RequestedCount { get; private set; } = 1;

        public static Url Create(string url, string alias)
        {
            return new Url(url, alias);
        }

        public void SetTinyUrl(string tinyUrl)
        {
            TinyUrl = tinyUrl;
        }

        public void Request()
        {
            RequestedCount++;
        }
    }
}

