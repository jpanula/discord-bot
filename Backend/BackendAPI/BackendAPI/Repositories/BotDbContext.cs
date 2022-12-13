using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repositories
{
    public class BotDbContext : DbContext
    {
        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Magic8BallResponse>().ToTable("Magic8BallResponse");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<EventVote>().ToTable("EventVote");
            modelBuilder.Entity<CommandGroup>().ToTable("CommandGroup");
            modelBuilder.Entity<SubCommand>().ToTable("SubCommand");
            modelBuilder.Entity<CommandParameter>().ToTable("CommandParameter");
        }
    }
}
