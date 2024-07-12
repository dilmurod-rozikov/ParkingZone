namespace ParkingZoneApp.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByID(Guid id);
        Task Add(T value);
        Task Update(T value);
        Task Delete(T value);
    }
}
