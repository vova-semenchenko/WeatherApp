using System.Text.Json;
using WeatherApp.DB;
using WeatherApp.Model;

namespace WeatherApp;
class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string apiKey = "3364a0b4121d1d46604a2bbd6a6f4fd6";
    private static readonly string cityName = "Cherkasy";
    private static readonly string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&appid={apiKey}";
    private static readonly string dataBasePath = "weather.db";


    async static Task Main(string[] args)
    {
        await getWeather();

        async Task getWeather()
        {
            Console.WriteLine("Getting JSON...");
            var responseString = await client.GetStringAsync(url);

            Console.WriteLine("Parsing JSON...");
            WeatherData? weatherForecast =
               JsonSerializer.Deserialize<WeatherData>(responseString);

            DataVilualization dataFromJson = new DataVilualization(weatherForecast);
            Console.WriteLine($"From JSON {dataFromJson}");

            DataBase(weatherForecast);
        }
    }


    static void DataBase(WeatherData? weatherData)
    {
        DatabaseManager database = new DatabaseManager(dataBasePath);

        database.InsertData(weatherData);

        Console.WriteLine("Reading Data From Data Base...");
        List<WeatherData> weatherDataFFromDatabase = database.ReadDataFromDataBase();

            
        int entryNumber = 0;
        foreach (WeatherData item in weatherDataFFromDatabase)
        {
            DataVilualization dataFromJson = new DataVilualization(item);
            
            Console.WriteLine($"Entry #{entryNumber++} \n {dataFromJson.ToString()}");
        }
    }
}

