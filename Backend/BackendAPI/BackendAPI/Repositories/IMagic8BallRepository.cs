using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public interface IMagic8BallRepository : IRepository<Magic8BallResponse>
    {
        public List<Magic8BallResponse> GetByType(Magic8BallResponse.AnswerType type);
        public Magic8BallResponse GetRandomByType(Magic8BallResponse.AnswerType type);

        /// <summary>
        /// Get a random response with the same probabilities as an original magic 8-ball:
        /// 50% affirmative, 25% negative, 25% noncommittal.
        /// </summary>
        /// <returns>Random magic 8-ball response.</returns>
        public Magic8BallResponse GetRandomWeighted();
    }
}
