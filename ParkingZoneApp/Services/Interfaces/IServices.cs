namespace ParkingZoneApp.Services.Interfaces
{
    public interface IServices<T> where T : class
    {
        void Insert(T value);

        void Update(T value);

        void Remove(Guid value);

        IEnumerable<T> RetrieveAll();

        T? GetById(Guid? value);

    }
}
