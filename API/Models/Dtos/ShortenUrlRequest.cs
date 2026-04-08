using System.ComponentModel.DataAnnotations;

namespace API.Models.Dtos
{
    public class ShortenUrlRequest
    {
        [Required(ErrorMessage = "Необходима URL ссылка")]
        [Url(ErrorMessage = "Некорректный формат ссылки")]
        public string Url { get; set; } = string.Empty;
    }
}
