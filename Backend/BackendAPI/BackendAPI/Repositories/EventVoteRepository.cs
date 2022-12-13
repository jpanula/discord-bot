using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public class EventVoteRepository : RepoBase<EventVote>, IEventVoteRepository
    {
        public EventVoteRepository(BotDbContext context) : base(context)
        {
        }
    }
}
