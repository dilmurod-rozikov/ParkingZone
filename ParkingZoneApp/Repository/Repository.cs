
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

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task Add(T value)
        {
            _dbSet.Add(value);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T value)
        {
            _dbSet.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByID(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Update(T value)
        {
            _context.Update(value);
            await _context.SaveChangesAsync();
        }
    }
}
