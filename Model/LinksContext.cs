using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Model
{
    public class LinksContext : DbContext
    {
        public DbSet<Link> Links { get; set; }
        public LinksContext(DbContextOptions<LinksContext> options)
            : base(options)
        {
            // Проверка на существование БД, если её нет, то Entity Framework создаст её
            Database.EnsureCreated();
           
        }

    }
}
