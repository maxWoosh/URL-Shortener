using API.Models.Entities;

namespace API.Repositories.Interfaces
{
    public interface IUrlRepository
    {
        // Получить ссылку по короткому коду
        Task<ShortenedUrl?> GetByCodeAsync(string shortCode);

        // Получить ссылку по оригинальному Url
        Task<ShortenedUrl?> GetByLongUrlAsync(string longUrl);

        // Создать новую ссылку
        Task<ShortenedUrl> CreateAsync(ShortenedUrl shortenedUrl);

        // Обновить существующую ссылку (для счетчика)
        Task UpdateAsync(ShortenedUrl shortenedUrl);

        // Проверить, существует ли код
        Task<bool> CodeExistsAsync(string shortCode);

        // Получить все сущности
        Task<IEnumerable<ShortenedUrl>> GetAllAsync();
    }
}
