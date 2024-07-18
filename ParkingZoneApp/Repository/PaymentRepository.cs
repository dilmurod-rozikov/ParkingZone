using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;

namespace ParkingZoneApp.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Payment> _dbSet;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Payment>();
        }
        public async Task<bool> StorePaymentDetails(Payment payment)
        {
            _dbSet.Add(payment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
