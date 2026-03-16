using API.Models.Entities;

namespace API.Services.Interfaces
{
    public interface IUrlService
    {
        // Создать короткую ссылку
        Task<ShortenedUrl> CreateShortUrlAsync(string longUrl);

        // Получить оригинальный URL по коду
        Task<string?> GetLongUrlAsync(string shortCode);

        // Увеличить счетчик кликов
        Task IncrementClickCountAsync(string shortCode);

        // Проверить наличие кода
        Task<bool> CodeExistsAsync(string shortCode);


    }
}
