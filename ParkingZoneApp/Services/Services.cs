using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class Services<T> : IServices<T> where T : class
    {
        private readonly IRepository<T> repository;

        public Services(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public void Insert(T value)
        {
            repository.Add(value);
        }

        public void Update(T value)
        {
            repository.Update(value);
        }

        public void Remove(T value)
        {
            repository.Delete(value);
        }

        public IEnumerable<T> RetrieveAll()
        {
            return repository.GetAll();
        }

        public T? GetById(Guid? value)
        {
            return repository.GetByID(value);
        }
    }
}
