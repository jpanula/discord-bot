using BackendAPI.Models;
using BackendAPI.Repositories;

namespace BackendAPI.Services
{
    public class Magic8BallService : IMagic8BallService
    {
        private readonly IMagic8BallRepository _repository;

        public Magic8BallService(IMagic8BallRepository repository)
        {
            _repository = repository;
        }

        public Magic8BallResponse Add(Magic8BallResponseData data)
        {
            var newResponse = new Magic8BallResponse();
            newResponse.Content = data.Content;
            newResponse.Type = data.Type;
            _repository.Add(newResponse);
            return _repository.GetLatest();
        }

        public Magic8BallResponse Delete(int id)
        {
            var response = _repository.GetById(id);
            if (response == null)
            {
                return null;
            }
            _repository.Delete(id);
            return response;
        }

        public List<Magic8BallResponse> Get()
        {
            return _repository.Get();
        }

        public Magic8BallResponse GetRandomWeighted()
        {
            return _repository.GetRandomWeighted();
        }
    }
}
