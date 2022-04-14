using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShorter.Data;
using UrlShorter.Models;
using UrlShorter.Helpers; 

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
            return _context.UrlModels.Where(x => x.Id == ConvertShortUrl.Decode(shortUrl)).FirstOrDefault();
        }

        public string UrlSave(string originalUrl)
        {
            UrlModel url = new UrlModel()
            {
                OriginalUrl = originalUrl
            };

            _context.UrlModels.Add(url);
            _context.SaveChanges();

            return ConvertShortUrl.Encode(url.Id);
        }

    }
}
