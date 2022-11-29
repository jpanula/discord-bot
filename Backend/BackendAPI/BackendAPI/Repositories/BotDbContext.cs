using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repositories
{
    public class BotDbContext : DbContext
    {
        public DbSet<Magic8BallResponse> Magic8BallResponses{ get; set; }

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Magic8BallResponse>().ToTable("Magic8BallResponse");
        }
    }
}
