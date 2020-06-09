using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Model
{
    public class OriginalLinkDTO
    {
        [Required(ErrorMessage = "Link field must not be empty")]

        public string OriginalLink { get; set; }
    }
}
