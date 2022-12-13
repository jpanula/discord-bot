using BackendAPI.Models;

namespace BackendAPI.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        public List<T> Get();
        public T GetById(int id);
        public T GetLatest();
        public void Add(T item);
        public void Update(int id, T item);
        public void Delete(int id);
    }
}
