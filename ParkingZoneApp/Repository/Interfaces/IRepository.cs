namespace ParkingZoneApp.Repository.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T? GetByID(Guid? id);
        void Add(T value);
        void Update(T value);
        void Delete(T value);
    }
}
