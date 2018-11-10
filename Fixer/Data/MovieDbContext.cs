using Fixer.Tmdb.Models;
using Microsoft.EntityFrameworkCore;

namespace Fixer.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext()
        {
        }

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
    }
}