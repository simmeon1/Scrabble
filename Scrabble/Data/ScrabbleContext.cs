using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Scrabble.Models
{
    public class ScrabbleContext : DbContext
    {
        public ScrabbleContext (DbContextOptions<ScrabbleContext> options)
            : base(options)
        {
        }

        public DbSet<Scrabble.Models.Game> Game { get; set; }
    }
}
