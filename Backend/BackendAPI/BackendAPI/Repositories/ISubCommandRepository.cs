using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public interface ISubCommandRepository : IRepository<SubCommand>
    {
        public List<CommandParameter> GetParameters(int id);
    }
}
