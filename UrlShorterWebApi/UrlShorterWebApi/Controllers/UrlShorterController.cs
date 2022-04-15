using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config; 
        static int hashLenght;


        public UrlShorterController(IUrlService service, IConfiguration configuration )
        {
            _service = service;
            _config = configuration;
         

            if (!int.TryParse(_config["HashLenght"], out hashLenght))
            {
                hashLenght = 6;
            }

        }

        /// <summary>
        /// Take a short URL and redirect to the original URL
        /// </summary> 
        [HttpGet("Redirection")]       
        public IActionResult Get(string shortUrl)
        {
            var url = _service.GetUrl(shortUrl);

            if (url != null)
            {
                //return Redirect(url.OriginalUrl);
                return Ok($"Redirecting...  {url.OriginalUrl} ");
            }

            return NotFound();
        }

        /// <summary>
        /// Take a URL and return a much shorter URL.
        /// </summary>
        ///     
        [HttpPost("Shortening")] 
        public IActionResult Post(string url)
        {
            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out Uri uriUrl)
              && !uriUrl.IsLoopback
              && (uriUrl.Scheme == Uri.UriSchemeHttp || uriUrl.Scheme == Uri.UriSchemeHttps);

            if (!isValidUrl)
                return BadRequest($"Invalid URL: {url}");


            var id = _service.UrlSave(url, hashLenght);
           
            return Ok($"Your shortened url is {Request.Scheme}://{Request.Host}/{id} ");
        }

        /// <summary>
        /// Allow the users to pick custom shortened URL. 
        /// </summary>  
        [HttpPost("CustomUrl")]
        public IActionResult Post(string url, string yourchoosedname)
        {
            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out Uri uriUrl)
             && !uriUrl.IsLoopback
             && (uriUrl.Scheme == Uri.UriSchemeHttp || uriUrl.Scheme == Uri.UriSchemeHttps);

            if (!isValidUrl)
                return BadRequest($"Invalid URL: {url}");

            if (yourchoosedname.ToCharArray().Length > hashLenght)
            {
                return BadRequest($"yourchoosedname can be up to {hashLenght} characters, {yourchoosedname}");

            }

            var id = _service.UrlSave(url, yourchoosedname, hashLenght);

            return Ok($"Your shortened url is {Request.Scheme}://{Request.Host}/{id} ");
        }
    }
}
