using API.Models.Entities;
using API.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace API.Repositories
{
    public class InMemoryUrlRepository : IUrlRepository
    {
        // Хранилище данных
        private readonly ConcurrentDictionary<string, ShortenedUrl> _urls = new();

        // Защита от одновременных запросов - неактуально (реализовал через ConcurrentDictionary)
        private readonly object _locker = new();

        public Task<ShortenedUrl?> GetByCodeAsync(string shortCode)
        {
            _urls.TryGetValue(shortCode, out var url);
            return Task.FromResult(url);

        }

        public Task<ShortenedUrl?> GetByLongUrlAsync(string longUrl)
        {
            var url = _urls.Values.FirstOrDefault(u => u.OriginalUrl == longUrl);
            return Task.FromResult(url);
        }

        public Task<ShortenedUrl> CreateAsync(ShortenedUrl shortenedUrl)
        {
            if (_urls.TryAdd(shortenedUrl.ShortCode, shortenedUrl))
                throw new InvalidOperationException($"Код {shortenedUrl.ShortCode} уже существует.");
            return Task.FromResult(shortenedUrl);
        }

        public Task UpdateAsync(ShortenedUrl shortenedUrl)
        {
            if (!_urls.ContainsKey(shortenedUrl.ShortCode))
                throw new InvalidOperationException($"Код {shortenedUrl.ShortCode} не найден.");
            _urls[shortenedUrl.ShortCode] = shortenedUrl;
            return Task.CompletedTask;
        }

        public Task<bool> CodeExistsAsync(string shortCode)
        {
            return Task.FromResult(_urls.ContainsKey(shortCode));
        }

        public Task<IEnumerable<ShortenedUrl>> GetAllAsync()
        {
            // Values возвращает snapshot на момент вызова
            return Task.FromResult(_urls.Values.AsEnumerable());
        }

    }
}
