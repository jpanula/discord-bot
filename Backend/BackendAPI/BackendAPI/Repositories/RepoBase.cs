using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repositories
{
    public abstract class RepoBase<T> : IRepository<T>
    {
        protected BotDbContext _context;

        public RepoBase(BotDbContext context)
        {
            _context = context;
        }

        public abstract List<T> Get();
        public abstract T GetById(int id);
        public abstract T GetLatest();

        public virtual void Add(T item)
        {
            _context.Add(item);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            _context.Remove(GetById(id));
            _context.SaveChanges();
        }

        public virtual void Update(int id, T item)
        {
            var baseItem = GetById(id);
            UpdateItem(ref baseItem, item);
            _context.Update(baseItem);
            _context.SaveChanges();
        }

        public virtual void UpdateItem(ref T baseItem, T newItem)
        {
            Type type = baseItem.GetType();

            foreach (var property in type.GetProperties())
            {
                property.SetValue(baseItem, property.GetValue(newItem));
            }
        }
    }
}
