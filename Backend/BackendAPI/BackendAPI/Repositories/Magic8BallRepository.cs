using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public class Magic8BallRepository : RepoBase<Magic8BallResponse>, IMagic8BallRepository
    {
        private readonly Random _rand;

        public Magic8BallRepository(BotDbContext context, Random random) : base(context)
        {
            _rand = random;
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

        public List<Magic8BallResponse> GetByType(Magic8BallResponse.AnswerType type)
        {
            return _context.Magic8BallResponses.Where(response => response.Type == type).ToList();
        }

        public Magic8BallResponse GetRandomByType(Magic8BallResponse.AnswerType type)
        {
            var responses = GetByType(type);
            var randomIndex = _rand.Next(responses.Count);
            return responses[randomIndex];
        }

        public Magic8BallResponse GetRandomWeighted()
        {
            var choiceValue = _rand.NextDouble();

            if (choiceValue < 0.5)
            {
                return GetRandomByType(Magic8BallResponse.AnswerType.Affirmative);
            }
            if (choiceValue < 0.75)
            {
                return GetRandomByType(Magic8BallResponse.AnswerType.Negative);
            }
            return GetRandomByType(Magic8BallResponse.AnswerType.Noncommittal);
        }
    }
}
