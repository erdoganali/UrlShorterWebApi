using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShorter.Data;
using UrlShorter.Models; 
using UrlShorter.Services;

namespace UrlShorter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UrlShorterController : Controller
    {
        private readonly IUrlService _service;
  
        public UrlShorterController(IUrlService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{shortUrl}")]
        public IActionResult Get(string shortUrl)
        {
            var url = _service.GetUrl(shortUrl);

            if (url != null)
            {
                return Redirect(url.OriginalUrl);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Post()
        {
            using var reader = new StreamReader(Request.Body);
            string url = reader.ReadToEnd();

            var shortURL = _service.UrlSave(url);

            return Ok($"shortURL");
        }

    }
}
