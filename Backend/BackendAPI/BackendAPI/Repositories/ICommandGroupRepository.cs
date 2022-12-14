using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public interface ICommandGroupRepository : IRepository<CommandGroup>
    {
        public List<SubCommand> GetSubcommands(int id);
    }
}
