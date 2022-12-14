using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repositories
{
    public class CommandGroupRepository : RepoBase<CommandGroup>, ICommandGroupRepository
    {
        public CommandGroupRepository(BotDbContext context) : base(context)
        {
        }

        public override CommandGroup GetById(int id)
        {
            return _context.Set<CommandGroup>().Include(item => item.SubCommands).FirstOrDefault(item => item.Id == id);
        }

        public List<SubCommand> GetSubcommands(int id)
        {
            return GetById(id).SubCommands.ToList();
        }
    }
}
