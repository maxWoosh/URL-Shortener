using API.Models.Entities;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utils;


namespace API.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly CodeGenerator _codeGenerator;

        // Зависимости приходят через конструктор
        public UrlService(IUrlRepository urlRepository, CodeGenerator codeGenerator)
        {
            _urlRepository = urlRepository;
            _codeGenerator = codeGenerator;
        }

        public async Task<ShortenedUrl> CreateShortUrlAsync(string longUrl)
        {
            // Проверяем, не существут ли такая ссылка
            var existing = await _urlRepository.GetByLongUrlAsync(longUrl);

            if (existing != null)
            {
                return existing;
            }

            // Генерируем уникальный код
            var shortCode = await GenerateUniqueCodeAsync();

            // Создаем сущность
            var shortenedUrl = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = longUrl,
                ShortCode = shortCode,
                CreatedAt = DateTime.UtcNow,
                ClickCount = 0
            };

            // Сохраняем в репозиторий
            await _urlRepository.CreateAsync(shortenedUrl);

            return shortenedUrl;

        }

        public async Task<string?> GetLongUrlAsync(string shortCode)
        {
            var url = await _urlRepository.GetByCodeAsync(shortCode); // Делегируем репозиторию
            return url?.OriginalUrl;
        }

        public async Task IncrementClickCountAsync(string shortCode)
        {
            // Находим ссылку через репозиторий
            var url = await _urlRepository.GetByCodeAsync(shortCode);

            if (url != null)
            {
                url.ClickCount++;

                // Сохраняем изменения через репозиторий
                await _urlRepository.UpdateAsync(url);
            }
        }

        private async Task<string> GenerateUniqueCodeAsync()
        {
            const int maxAttempts = 10;
            int attempt = 0;

            while (attempt < maxAttempts)
            {
                var code = _codeGenerator.Generate(); // Генерируем 6-значный код

                if (!await CodeExistsAsync(code))
                {
                    return code;
                }

                attempt++;
            }

            throw new Exception($"Ошибка генерации уникального кода. Число попыток = {maxAttempts}");
        }

        public async Task<bool> CodeExistsAsync(string shortCode)
        {
            return await _urlRepository.CodeExistsAsync(shortCode); // Делегируем репозиторию
        }

    }
}
