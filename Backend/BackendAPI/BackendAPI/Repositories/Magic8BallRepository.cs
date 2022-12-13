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

        public List<Magic8BallResponse> GetByType(Magic8BallResponse.AnswerType type)
        {
            return _context.Set<Magic8BallResponse>().Where(response => response.Type == type).ToList();
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
