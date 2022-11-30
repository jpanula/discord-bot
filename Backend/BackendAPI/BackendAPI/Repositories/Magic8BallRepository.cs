using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public class Magic8BallRepository : RepoBase<Magic8BallResponse>, IMagic8BallRepository
    {
        public Magic8BallRepository(BotDbContext context) : base(context)
        {
        }

        public override List<Magic8BallResponse> Get()
        {
            return _context.Magic8BallResponses.ToList();
        }

        public override Magic8BallResponse GetById(int id)
        {
            return _context.Magic8BallResponses.FirstOrDefault(item => item.Id == id);
        }

        public override Magic8BallResponse GetLatest()
        {
            return _context.Magic8BallResponses.OrderByDescending(item => item.Id).First();
        }
    }
}
