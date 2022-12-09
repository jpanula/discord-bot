using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        public List<EventVote> GetVotes(int id);
        public Event GetEventFromMessageId(string messageId);
    }
}
