using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

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

        public async Task<List<Route>> GetRouteInformationList(List<Route> routeList, string pickUpLocation, string dropOffLocation)
        {
            var client = new HttpClient();

            var dropOffRide = await CalculateDropOffRide(client, pickUpLocation, dropOffLocation);

            var pickUpRides = CalculatePickUpRide(client, routeList, pickUpLocation);
            var pickUpRideIterator = pickUpRides.GetAsyncEnumerator();

            int i;
            for (i = 0; await pickUpRideIterator.MoveNextAsync(); i++)
            {
                var direction = new Direction();

                if (pickUpRideIterator.Current.Distance.Value != -1)
                {
                    direction.PickUpRide = pickUpRideIterator.Current;
                    direction.DropOffRide = dropOffRide;
                    direction.MapLink = $"https://www.google.com/maps/dir/{ routeList[i].Car.Location },+USA/{ pickUpLocation },+USA/{ dropOffLocation },+USA";
                }

                routeList[i].Direction = direction;
            }
            for(i += 1; i < routeList.Count; i++)
            {
                routeList[i].Direction = new Direction();
            }
            
            return routeList;
        }

        private async Task<Ride> CalculateDropOffRide(HttpClient client, string pickUpLocation, string dropOffLocation)
        {
            var dropOffRide = new Ride();
            if (pickUpLocation != "" && dropOffLocation != "")
            {
                dynamic json = await GetJson(client, BuildAddress(pickUpLocation, dropOffLocation));

                if (json.status == "OK" && json.rows[0].elements[0].status == "OK")
                {
                    dynamic ride = json.rows[0].elements[0];

                    dropOffRide = CopyDynamicRide(ride);
                }
            }

            return dropOffRide;
        }

        private async IAsyncEnumerable<Ride> CalculatePickUpRide(HttpClient client, List<Route> routeDict, string pickUpLocation)
        {
            if(routeDict.Count > 0 && pickUpLocation != "")
            {
                var carLocations = MergeLocations(routeDict);
                dynamic json = await GetJson(client, BuildAddress(carLocations, pickUpLocation));

                if(json.status == "OK")
                {
                    foreach (var row in json.rows)
                    {
                        var ride = row.elements[0];

                        if (ride.status == "OK")
                        {
                            var newRide = CopyDynamicRide(ride);

                            yield return newRide;
                        }
                        else
                            yield return new Ride();
                    }
                }
            }

            yield break;
        }

        private static string MergeLocations(List<Route> routeDict)
        {
            var locationStringBuilder = new StringBuilder();

            foreach (var route in routeDict)
            {
                locationStringBuilder.Append(route.Car.Location + '|');
            }
            if(locationStringBuilder.Length > 0)
                locationStringBuilder.Remove(locationStringBuilder.Length - 1, 1);

            return locationStringBuilder.ToString();
        }

        private string BuildAddress(string origins, string destinations)
        {
            var address = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origins}&destinations={destinations}&units=imperial&key={_config.GetConnectionString(GoogleMapsApiKey)}";
            address = Regex.Replace(address, @"\s+", String.Empty);
            return address;
        }

        private static async Task<dynamic> GetJson(HttpClient client, string address)
        {
            var jsonString = await client.GetStringAsync(address);
            return JsonConvert.DeserializeObject(jsonString);
        }

        private static Ride CopyDynamicRide(dynamic ride)
        {
            Ride newRide = new();

            newRide.Distance.Value = ride.distance.value;
            newRide.Distance.Text = ride.distance.text;

            newRide.Duration.Value = ride.duration.value;
            newRide.Duration.Text = ride.duration.text;

            return newRide;
        }
    }
}
