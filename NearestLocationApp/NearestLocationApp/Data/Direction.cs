using DataAccessLibrary;

namespace NearestLocationApp.Data
{
    public struct Direction
    {
        public Direction(Car car)
        {
            Car = car;
            ToPickUpRide = new Ride();
            ToDropOffRide = new Ride();
            MapLink = "";
        }

        public Car Car { get; set; }
        public Ride ToPickUpRide { get; set; }
        public Ride ToDropOffRide { get; set; }
        public string MapLink { get; set; }
    }
}
