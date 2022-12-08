using BackendAPI.Models;

namespace BackendAPI.Services
{
    public interface IEventService
    {
        public List<Event> Get();
        public Event GetById(int id);
        public Event Add(EventData data);
        public EventVote AddVote(int eventId, EventVoteData data);
        public Event Delete(int id);
        public EventVote DeleteVote(int eventId, EventVoteData data);
    }
}
