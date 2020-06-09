using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Model
{
    public class Link
    {
        public int Id { get; set; }

        public string OriginalLink { get; set; }

        public string ShortLink { get; set; }

        public DateTime DateOfCreation { get; set; }

    }
}
