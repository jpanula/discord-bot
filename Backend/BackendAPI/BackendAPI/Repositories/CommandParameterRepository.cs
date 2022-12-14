using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public class CommandParameterRepository : RepoBase<CommandParameter>, ICommandParameterRepository
    {
        public CommandParameterRepository(BotDbContext context) : base(context)
        {
        }
    }
}
