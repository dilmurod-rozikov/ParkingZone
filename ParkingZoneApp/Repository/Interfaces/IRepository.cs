namespace ParkingZoneApp.Repository.Interfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetByID(Guid? id);
        void Add(T value);
        void Update(T value);
        T Delete(Guid Id);
    }
}
