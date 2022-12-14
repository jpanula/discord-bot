using BackendAPI.Models;

namespace BackendAPI.Services
{
    public interface ICommandService
    {
        public List<CommandGroup> GetCommandGroups();
        public CommandGroup GetCommandGroupById(int id);
        public SubCommand GetSubCommandById(int id);
    }
}
