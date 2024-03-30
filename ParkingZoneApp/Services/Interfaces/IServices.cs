namespace ParkingZoneApp.Services.Interfaces
{
    public interface IServices<T> where T : class
    {
        void Insert(T value);

        void Update(T value);

        void Remove(T value);

        IEnumerable<T> RetrieveAll();

        T? GetById(Guid? value);

    }
}
