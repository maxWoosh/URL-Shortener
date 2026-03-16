using API.Models.Entities;
using API.Services.Interfaces;
using API.Utils;


namespace API.Services
{
    public class UrlService : IUrlService
    {
        //private static readonly Dictionary<string, ShortenedUrl> _urlsVault = new ();

        private readonly CodeGenerator _codeGenerator;

        public UrlService(CodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
        }

        //public Task<ShortenedUrl> CreateShortUrlAsync(string longUrl)
        //{
            
        //}

    }
}
