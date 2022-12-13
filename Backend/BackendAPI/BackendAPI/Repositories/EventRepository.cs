using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repositories
{
    public class EventRepository : RepoBase<Event>, IEventRepository
    {
        public EventRepository(BotDbContext context) : base(context)
        {
        }

        public List<EventVote> GetVotes(int id)
        {
            return GetById(id).Votes.ToList();
        }

        public Event GetEventFromMessageId(string messageId)
        {
            return _context.Set<Event>().Include(_item => _item.Votes).FirstOrDefault(item => item.MessageIds.Contains(messageId));
        }

        public override Event GetById(int id)
        {
            return _context.Set<Event>().Include(item => item.Votes).FirstOrDefault(item => item.Id == id);
        }
    }
}
