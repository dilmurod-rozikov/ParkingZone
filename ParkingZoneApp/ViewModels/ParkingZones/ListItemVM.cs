namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class ListItemVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; init; }

       
    }
}
