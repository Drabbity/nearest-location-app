using DataAccessLibrary;

namespace NearestLocationApp.Data
{
    public class Ride
    {
        public Ride()
        {
            ResetValues();
        }

        public void ResetValues()
        {
            DistanceString = "N/A";
            DistanceValue = 0;
            DurationString = "N/A";
            DurationValue = 0;
        }

        public string DistanceString { get; set; }
        public int DistanceValue { get; set; }

        public string DurationString { get; set; }
        public int DurationValue { get; set; }
    }
}
