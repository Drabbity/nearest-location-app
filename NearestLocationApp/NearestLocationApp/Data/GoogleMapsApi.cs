using DataAccessLibrary;
using Newtonsoft.Json;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq.Expressions;
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



        public async Task<List<(Cordinate, Ride)>> GetRideInformation(List<(Cordinate, Ride)> destinations, Cordinate origin)
        {
            if(origin.Latitude != "" && origin.Longitude != "")
            {
                var destinationsStringBuilder = new StringBuilder();

                foreach (var destination in destinations)
                {
                    destinationsStringBuilder.Append(destination.Item1.Latitude + "," + destination.Item1.Longitude + '|');
                }
                destinationsStringBuilder.Remove(destinationsStringBuilder.Length - 1, 1);

                var originString = origin.Latitude + "," + origin.Longitude;

                string address = $"https://maps.googleapis.com/maps/api/distancematrix/json?destinations={destinationsStringBuilder.ToString()}&origins={originString}&key={ _config.GetConnectionString(GoogleMapsApiKey) }";
                address = Regex.Replace(address, @"\s+", String.Empty);

                HttpClient client = new HttpClient();
                string jsonString = await client.GetStringAsync(address);

                dynamic json = JsonConvert.DeserializeObject(jsonString);

                if(json.status == "OK")
                {
                    int index = 0;

                    foreach (var element in json.rows[0].elements)
                    {
                        if (element.status == "OK")
                        {
                            destinations[index].Item2.DistanceValue = element.distance.value;
                            destinations[index].Item2.DistanceString = element.distance.text;

                            destinations[index].Item2.DurationValue = element.duration.value;
                            destinations[index].Item2.DurationString = element.duration.text;
                        }
                        else
                        {
                            destinations[index].Item2.ResetValues();
                        }
                        index++;
                    }
                }                
            }
            else
            {
                foreach(var destination in destinations)
                {
                    destination.Item2.ResetValues();
                }
            }

            return destinations;
        }
    }
}