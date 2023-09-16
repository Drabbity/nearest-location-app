using DataAccessLibrary;

namespace NearestLocationApp.Data
{
    public class Route
    {
        public Car Car { get; set; } = new Car();
        public Direction Direction { get; set; } = new Direction();
    }
}
