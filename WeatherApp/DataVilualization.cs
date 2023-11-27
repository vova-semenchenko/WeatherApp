using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp
{
    public class DataVilualization
    {
        private readonly WeatherData? weatherData;
       
        public DataVilualization(WeatherData? weatherData)
        {
            this.weatherData = weatherData;
        }

        private string DataPreparing()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"City: {weatherData?.name}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Weather: {weatherData?.weather?[0].description}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Temp: {weatherData?.main?.temp}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Feels Like: {weatherData?.main?.feels_like}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Temp Min: {weatherData?.main?.temp_min}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Temp Max: {weatherData?.main?.temp_max}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Visibility: {weatherData?.visibility}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Wind Speed: {weatherData?.wind?.speed}");
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return $"------------------\n{DataPreparing()}------------------\n";
        }
    }
}
