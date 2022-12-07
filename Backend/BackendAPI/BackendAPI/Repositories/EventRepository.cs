using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public class EventRepository : RepoBase<Event>, IEventRepository
    {
        public EventRepository(BotDbContext context) : base(context)
        {
        }

        public override List<Event> Get()
        {
            return _context.Events.ToList();
        }

        public override Event GetById(int id)
        {
            return _context.Events.FirstOrDefault(item => item.Id == id);
        }

        public override Event GetLatest()
        {
            return _context.Events.OrderByDescending(item => item.Id).First();
        }
    }
}
