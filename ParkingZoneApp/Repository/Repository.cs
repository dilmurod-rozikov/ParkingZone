
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Data;
using ParkingZoneApp.Repository.Interfaces;

namespace ParkingZoneApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public void Add(T value)
        {
            _dbSet.Add(value);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);

            if (entity != null) 
                _dbSet.Remove(entity);
                
            _context.SaveChanges();
        }

        public T? GetByID(Guid? id)
        {
            var entity = _dbSet.Find(id);
            return entity;
        }

        public void Update(T value)
        {
            _context.Update(value);
            _context.SaveChanges();
        }
    }
}
