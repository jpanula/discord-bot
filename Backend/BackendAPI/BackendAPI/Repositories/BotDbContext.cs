using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repositories
{
    public class BotDbContext : DbContext
    {
        public DbSet<Magic8BallResponse> Magic8BallResponses{ get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventVote> EventVotes { get; set; }
        public DbSet<CommandGroup> CommandGroups { get; set; }
        public DbSet<SubCommand> SubCommands { get; set; }
        public DbSet<CommandParameter> CommandParameters { get; set; }

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
