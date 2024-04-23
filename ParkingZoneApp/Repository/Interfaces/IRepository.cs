namespace ParkingZoneApp.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetByID(Guid id);
        void Add(T value);
        void Update(T value);
        void Delete(T value);
    }
}
