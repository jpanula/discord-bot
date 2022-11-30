using BackendAPI.Models;

namespace BackendAPI.Services
{
    public interface IMagic8BallService
    {
        public List<Magic8BallResponse> Get();
        public Magic8BallResponse Add(string response);
        public Magic8BallResponse Delete(int id);
    }
}
