using System;

namespace UrlShorter.Models
{
    public class UrlModel
    {
         
        public int Id { get; set; }
        public string Url { get; set; }
        public string OriginalUrl { get; set; } 
        public DateTime DueDate { get; set; }
         
    }
}
