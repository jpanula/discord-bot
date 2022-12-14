using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repositories
{
    public class SubCommandRepository : RepoBase<SubCommand>, ISubCommandRepository
    {
        public SubCommandRepository(BotDbContext context) : base(context)
        {
        }

        public override SubCommand GetById(int id)
        {
            return _context.Set<SubCommand>().Include(item => item.Parameters).FirstOrDefault(item => item.Id == id);
        }

        public List<CommandParameter> GetParameters(int id)
        {
            return GetById(id).Parameters.ToList();
        }
    }
}
