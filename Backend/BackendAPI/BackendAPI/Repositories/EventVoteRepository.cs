using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public class EventVoteRepository : RepoBase<EventVote>, IEventVoteRepository
    {
        public EventVoteRepository(BotDbContext context) : base(context)
        {
        }

        public override List<EventVote> Get()
        {
            return _context.EventVotes.ToList();
        }

        public override EventVote GetById(int id)
        {
            return _context.EventVotes.FirstOrDefault(item => item.Id == id);
        }

        public override EventVote GetLatest()
        {
            return _context.EventVotes.OrderByDescending(item => item.Id).First();
        }
    }
}
