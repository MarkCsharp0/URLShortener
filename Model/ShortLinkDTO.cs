using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Model
{
    public class ShortLinkDTO
    {
        [Required]
        public string ShortLink { get; set; }

    }
}
