using BackendAPI.Models;

namespace BackendAPI.Services
{
    public interface IMagic8BallService
    {
        public List<Magic8BallResponse> Get();
        public Magic8BallResponse GetRandomWeighted();
        public Magic8BallResponse Add(Magic8BallResponseData data);
        public Magic8BallResponse Delete(int id);
    }
}
