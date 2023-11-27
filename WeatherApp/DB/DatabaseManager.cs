using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.DB
{
    public class DatabaseManager
    {
        private readonly string connectionString;
        private string dataBaseName;
        private SQLiteConnection connection;
       
        public DatabaseManager(string connectionStrinbg)
        {
            this.dataBaseName = connectionStrinbg;
            this.connectionString = $"Data Source={connectionStrinbg};Version=3;";
            this.connection = new SQLiteConnection(connectionString);

            InitializeDatabase();
        }

       private void InitializeDatabase()
       {
            if (!File.Exists($".\\{dataBaseName}"))
            {
                SQLiteConnection.CreateFile(dataBaseName);
            }

            try
            {
                connection.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS weather_data (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    temp REAL,
                    feels_like REAL,
                    temp_min REAL,
                    temp_max REAL,
                    visibility INTEGER,
                    wind_speed REAL,
                    weather_description TEXT,
                    city_name TEXT
                );";

                SQLiteCommand command = new SQLiteCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
            }   
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
          
       }

        public void InsertData(WeatherData? weatherData)
        {
            if (weatherData == null)
                return;

            string insertDataQuery = @"
            INSERT INTO weather_data (
                temp, feels_like, temp_min, temp_max, visibility, wind_speed, weather_description, city_name
            ) VALUES (
                @temp, @feels_like, @temp_min, @temp_max, @visibility, @wind_speed, @weather_description, @city_name
            );";

            using (SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection))
            {
                command.Parameters.AddWithValue("@temp", weatherData.main?.temp);
                command.Parameters.AddWithValue("@feels_like", weatherData.main?.feels_like);
                command.Parameters.AddWithValue("@temp_min", weatherData.main?.temp_min);
                command.Parameters.AddWithValue("@temp_max", weatherData.main?.temp_max);
                command.Parameters.AddWithValue("@visibility", weatherData.visibility);
                command.Parameters.AddWithValue("@wind_speed", weatherData.wind?.speed);
                command.Parameters.AddWithValue("@weather_description", weatherData.weather?[0].description);
                command.Parameters.AddWithValue("@city_name", weatherData.name);

                command.ExecuteNonQuery();
            }
        }

        public List<WeatherData> ReadDataFromDataBase()
        {
            List<WeatherData> weatherDatas = new List<WeatherData>();

            try
            {

                string selectDataQuery = @"SELECT * FROM weather_data;";


                using (SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection))
                {

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            WeatherData weatherData = new WeatherData
                            {
                                main = new Main
                                {
                                    temp = Convert.ToDouble(reader["temp"]),
                                    feels_like = Convert.ToDouble(reader["feels_like"]),
                                    temp_min = Convert.ToDouble(reader["temp_min"]),
                                    temp_max = Convert.ToDouble(reader["temp_max"])
                                },
                                visibility = Convert.ToInt32(reader["visibility"]),
                                wind = new Wind { speed = Convert.ToDouble(reader["wind_speed"]) },
                                weather = new List<Weather>
                                {
                                new Weather { description = reader["weather_description"].ToString() }
                                },
                                name = reader["city_name"].ToString()
                            };


                            weatherDatas.Add(weatherData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data from database: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return weatherDatas;
        }       
    }
}
