
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
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet;

        public void Add(T value)
        {
            _dbSet.Add(value);
            _context.SaveChanges();
        }

        public void Delete(T value)
        {
            _dbSet.Remove(value);
            _context.SaveChanges();
        }

        public T? GetByID(Guid id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T value)
        {
            _context.Update(value);
            _context.SaveChanges();
        }
    }
}
