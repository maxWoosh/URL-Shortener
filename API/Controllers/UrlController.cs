using API.Models.Dtos;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;
        private readonly IConfiguration _configuration;

        public UrlController(IUrlService urlService, IConfiguration configuration)
        {
            _urlService = urlService;
            _configuration = configuration;
        }

        [HttpPost("shorten")]
        [ProducesResponseType(StatusCodes.Status201Created)] // Для Swagger
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShortenUrlResponse>> Shorten([FromBody] ShortenUrlRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shortenedUrl = await _urlService.CreateShortUrlAsync(request.Url);

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var shortUrl = $"{baseUrl}/api/url/{shortenedUrl.ShortCode}";

            var response = new ShortenUrlResponse
            {
                ShortCode = shortenedUrl.ShortCode,
                OriginalUrl = shortenedUrl.OriginalUrl,
                ShortUrl = shortUrl
            };

            return CreatedAtAction(nameof(RedirectToOriginal), new { shortCode = shortenedUrl.ShortCode }, response);
        }


        [HttpGet("{shortCode}")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            var longUrl = await _urlService.GetLongUrlAsync(shortCode);

            if (longUrl == null)
            {
                return NotFound(new { error = $"Код '{shortCode}' не найден" });
            }

            _ = Task.Run(() => _urlService.IncrementClickCountAsync(shortCode));

            return Redirect(longUrl);
        }
    }
}
