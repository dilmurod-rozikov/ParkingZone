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

        public virtual void Insert(T value)
        {
            _repository.Add(value);
        }

        public void Update(T value)
        {
            _repository.Update(value);
        }

        public void Remove(T value)
        {
            _repository.Delete(value);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(Guid? value)
        {
            return _repository.GetByID(value);
        }
    }
}
