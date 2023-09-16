
namespace NearestLocationApp.Data
{
    public class Direction
    {
        public Ride PickUpRide { get; set; } = new Ride();
        public Ride DropOffRide { get; set; } = new Ride();
        public string MapLink { get; set; }
    }
}
