using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherClientTool
{
    class APIService
    {

        public async Task<HttpResponseMessage> GetCityInformationAsync(string path, string lat, string lng)
        {
            string newPath = path.Replace("@lat", lat).Replace("@lng", lng);

            HttpResponseMessage message;

            using (HttpClient httpClient = new HttpClient())
            {

                message = await httpClient.GetAsync(newPath);
            }

            return message;
        }
    }
}
