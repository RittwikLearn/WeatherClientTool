using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherClientTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var appStartpath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string dataFilePath = appStartpath + "\\Data\\IndianLocations.csv";
            string apiUrl = @"https://api.open-meteo.com/v1/forecast?latitude=@lat&longitude=@lng&current_weather=true";
            Console.WriteLine("Custom Weather Client Tool, enter a city name");

            string cityName = "";

            cityName = Console.ReadLine();

            if (cityName != null && !string.IsNullOrEmpty(cityName))
            {
                DataTable cities = new DataOperation().GetDataFromCSV(dataFilePath);

                string? latitude = null;
                string? longitude = null;
                foreach (DataRow dataRow in cities.Rows)
                {
                    string checkCity = new DataOperation().removeExtra(dataRow["city"].ToString().Trim().ToLower());
                    if (checkCity == cityName.Trim().ToLower())
                    {
                        latitude = dataRow.Field<String>("lat");
                        longitude = dataRow.Field<String>("lng");
                        break;
                    }
                    else if(checkCity.Contains(cityName.Trim().ToLower()))
                    {
                        latitude = dataRow.Field<String>("lat");
                        longitude = dataRow.Field<String>("lng");
                        break;
                    }
                }

                if (latitude != null && longitude != null)
                {
                    APIService apiService = new APIService();

                    var information = await apiService.GetCityInformationAsync(apiUrl, latitude, longitude);
                    var info = await information.Content.ReadAsStringAsync();
                    dynamic infoParse = JObject.Parse(info);
                    Console.WriteLine("Latitude: " + infoParse.latitude);
                    Console.WriteLine("Longitude: " + infoParse.longitude);
                    Console.WriteLine("Generationtime_ms: " + infoParse.generationtime_ms);
                    Console.WriteLine("Utc_offset_seconds: " + infoParse.utc_offset_seconds);
                    Console.WriteLine("Timezone: " + infoParse.timezone);
                    Console.WriteLine("Timezone_abbreviation: " + infoParse.timezone_abbreviation);
                    Console.WriteLine("Elevation: " + infoParse.elevation);

                    dynamic currWeather = JObject.Parse(infoParse.current_weather.ToString());

                    Console.WriteLine("Current_weather ");
                    Console.WriteLine("\t Temperature: " + currWeather.temperature);
                    Console.WriteLine("\t Windspeed: " + currWeather.windspeed);
                    Console.WriteLine("\t Winddirection: " + currWeather.winddirection);
                    Console.WriteLine("\t Weathercode: " + currWeather.weathercode);
                    Console.WriteLine("\t Time: " + currWeather.time);

                }
                else
                {
                    Console.WriteLine("Sorry!!!Entered city name is not found.");
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid city name");
            }
            
        }
        
    }
}
