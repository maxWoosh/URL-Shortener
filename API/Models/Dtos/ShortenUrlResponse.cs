namespace API.Models.Dtos
{
    public class ShortenUrlResponse
    {
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
    }
}
