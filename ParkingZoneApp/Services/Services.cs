using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class Services<T> : IServices<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Services(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task Insert(T value)
        {
            await _repository.Add(value);
        }

        public async Task Update(T value)
        {
            await _repository.Update(value);
        }

        public async Task Remove(T value)
        {
            await _repository.Delete(value);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<T> GetById(Guid value)
        {
            return await _repository.GetByID(value);
        }
    }
}
