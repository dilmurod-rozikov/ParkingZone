
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Data;
using ParkingZoneApp.IRepository;

namespace ParkingZoneApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context = null;

        private readonly DbSet<T> _dbSet = null;

        public Repository()
        {
            this._context = new ApplicationDbContext();
            _dbSet = _context.Set<T>();
        }

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T value)
        {
            _dbSet.Add(value);
            _context.SaveChanges();
        }

        public T Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public List<T> GetAll() => _dbSet.ToList();
        
         public T GetByID(Guid? id)
             => _dbSet.Find(id);
        

        public void Update(T value)
        {
            _context.Update(value);
            _context.SaveChanges();
        }
    }
}
