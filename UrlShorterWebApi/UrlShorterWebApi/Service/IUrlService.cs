using UrlShorter.Models;

namespace UrlShorter.Services
{
    public interface IUrlService
    {
        UrlModel GetUrl(string path);

        string UrlSave(string originalUrl);
    }
}