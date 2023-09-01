using DataAccessLibrary;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace NearestLocationApp.Data
{
    public class GoogleMapsApi
    {
        public string GoogleMapsApiKey { get; set; } = "DefaultGoogleMapsApiKey";

        private readonly IConfiguration _config;

        public GoogleMapsApi(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Direction>> GetRideInformation(List<Direction> cars, string pickUpZipCode, string dropOffZipCode)
        {
            if (pickUpZipCode == "" || cars.Count == 0)
                return ResetValues(cars);
            
            HttpClient client = new HttpClient();
            string address;
            dynamic json;

            Ride toDropOffRide = new Ride();
            if(dropOffZipCode != "")
            {
                address = BuildAddress(pickUpZipCode, dropOffZipCode);
                json = await GetJson(client, address);

                if (json.status == "OK" && json.rows[0].elements[0].status == "OK")
                {
                    dynamic ride = json.rows[0].elements[0];

                    toDropOffRide.DistanceValue = ride.distance.value;
                    toDropOffRide.DistanceString = ride.distance.text;

                    toDropOffRide.DurationValue = ride.duration.value;
                    toDropOffRide.DurationString = ride.duration.text;
                }
            }  

            address = BuildAddress(MergeZipCodes(cars), pickUpZipCode);
            json = await GetJson(client, address);

            if (json.status != "OK")
                return ResetValues(cars);

            int index = 0;
            foreach (var row in json.rows)
            {
                var element = row.elements[0];

                if (element.status == "OK")
                {
                    Direction direction = new Direction(cars[index].Car);

                    direction.ToPickUpRide.DistanceValue = element.distance.value;
                    direction.ToPickUpRide.DistanceString = element.distance.text;

                    direction.ToPickUpRide.DurationValue = element.duration.value;
                    direction.ToPickUpRide.DurationString = element.duration.text;

                    direction.ToDropOffRide.DistanceValue = toDropOffRide.DistanceValue;
                    direction.ToDropOffRide.DistanceString = toDropOffRide.DistanceString;

                    direction.ToDropOffRide.DurationValue = toDropOffRide.DurationValue;
                    direction.ToDropOffRide.DurationString = toDropOffRide.DurationString;

                    direction.MapLink = $"https://www.google.com/maps/dir/{cars[index].Car.ZipCode},+USA/{pickUpZipCode},+USA/{dropOffZipCode},+USA";
                    
                    cars[index] = direction;
                }
                else
                {
                    cars[index].ToPickUpRide.ResetValues();
                    cars[index].ToDropOffRide.ResetValues();
                }
                index++;
            }

            return cars;
        }

        private string MergeZipCodes(List<Direction> cars)
        {
            var zipCodeStringBuilder = new StringBuilder();

            foreach (var car in cars)
            {
                zipCodeStringBuilder.Append(car.Car.ZipCode + '|');
            }
            zipCodeStringBuilder.Remove(zipCodeStringBuilder.Length - 1, 1);

            return zipCodeStringBuilder.ToString();
        }

        private string BuildAddress(string origins, string destinations)
        {
            string address = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origins}&destinations={destinations}&units=imperial&key={_config.GetConnectionString(GoogleMapsApiKey)}";
            address = Regex.Replace(address, @"\s+", String.Empty);
            return address;
        }

        private async Task<dynamic> GetJson(HttpClient client, string address)
        {
            string jsonString = await client.GetStringAsync(address);
            return JsonConvert.DeserializeObject(jsonString);
        }

        private List<Direction> ResetValues(List<Direction> cars)
        {
            foreach (var car in cars)
            {
                car.ToPickUpRide.ResetValues();
                car.ToDropOffRide.ResetValues();
            }

            return cars;
        }
    }
}
