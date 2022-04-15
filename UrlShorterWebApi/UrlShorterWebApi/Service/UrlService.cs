using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShorter.Data;
using UrlShorter.Models; 
using Microsoft.Extensions.Configuration;

namespace UrlShorter.Services
{
    public class UrlService: IUrlService
    { 
        private readonly UrlDbContext _context; 

        public UrlService(UrlDbContext urlContext)
        { 
            _context = urlContext;
        }

        public UrlModel GetUrl(string shortUrl)
        {
            shortUrl = shortUrl.Trim(' ');
            var arr = shortUrl.Split('/');
            Array.Reverse(arr);
            var key = arr[0];

            return _context.UrlModels.Where(x => x.Url == key).FirstOrDefault();
        }

        public string UrlSave(string originalUrl, int hashLenght)
        {
            originalUrl = originalUrl.Trim(' ');

            String newKey = null;

            if (!_context.UrlModels.Any(s => s.OriginalUrl == originalUrl))
            {
               
                newKey = Guid.NewGuid().ToString("N").Substring(0, hashLenght).ToLower(); 
                _context.UrlModels.Add( new UrlModel { Url = newKey, OriginalUrl= originalUrl, DueDate = DateTime.Now });
                _context.SaveChanges();
            }
            else
            {
                var shortpath = _context.UrlModels.Where(s => s.OriginalUrl == originalUrl).FirstOrDefault();
                if (shortpath != null)
                {
                    newKey = shortpath.Url;
                }
            } 

            return newKey;
        }

        public string UrlSave(string originalUrl, string yourchoosedname, int hashLenght)
        {

            String newKey = yourchoosedname;

            if (!_context.UrlModels.Any(s => s.OriginalUrl == originalUrl))
            { 
                _context.UrlModels.Add(new UrlModel { Url = newKey, OriginalUrl = originalUrl, DueDate = DateTime.Now });
                _context.SaveChanges();
            }
            else
            {
                var shortpath = _context.UrlModels.Where(s => s.OriginalUrl == originalUrl).FirstOrDefault();
                if (shortpath != null)
                {
                    newKey = shortpath.Url;
                }
            }

            return newKey;
        }

    }
}
