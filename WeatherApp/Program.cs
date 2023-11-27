using System.Text.Json;
using WeatherApp.Model;

namespace WeatherApp;
class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string cityName = "Dnipro";
    private static readonly string dataBasePath = "weather.db";


    async static Task Main(string[] args)
    {
        string? apiKey = Environment.GetEnvironmentVariable("API_KEY");
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&appid={apiKey}";

        await GetWeather();

        async Task GetWeather()
        {
            Console.WriteLine("Getting JSON...");
            var responseString = await client.GetStringAsync(url);

            Console.WriteLine("Parsing JSON...");
            WeatherData? weatherForecast =
               JsonSerializer.Deserialize<WeatherData>(responseString);

            DataVilualization dataFromJson = new DataVilualization(weatherForecast);
            Console.WriteLine($"Object f rom JSON {dataFromJson}");

            DataBase(weatherForecast);
        }
    }


    static void DataBase(WeatherData? weatherData)
    {
        DatabaseManager database = new DatabaseManager(dataBasePath);

        database.InsertData(weatherData);
        Console.WriteLine("Entry added to database!");

        Console.WriteLine("Reading data from database...");
        List<WeatherData> weatherDataFFromDatabase = database.ReadDataFromDataBase();

            
        int entryNumber = 0;
        foreach (WeatherData item in weatherDataFFromDatabase)
        {
            DataVilualization dataFromJson = new DataVilualization(item);
            
            Console.WriteLine($"Entry #{entryNumber++} \n {dataFromJson.ToString()}");
        }
    }
}

