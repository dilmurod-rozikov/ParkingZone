namespace ParkingZoneApp.Services.Interfaces
{
    public interface IServices<T> where T : class
    {
        Task Insert(T value);

        Task Update(T value);

        Task Remove(T value);

        Task<IEnumerable<T>> GetAll();

        Task<T?> GetById(Guid value);
    }
}
