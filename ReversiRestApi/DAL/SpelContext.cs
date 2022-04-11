using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ReversiRestApi.DAL
{
    public class SpelContext : DbContext
    {
        public SpelContext(DbContextOptions options) : base(options) { }

        public DbSet<Spel> Spellen { get; set; }
    }
}
