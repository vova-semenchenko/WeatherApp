using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public class WeatherData
    {
        public Coord? coord { get; set; }

        public IList<Weather>? weather { get; set; }


        [JsonProperty("base")]
        public string? Base { get; set; }


        public Main? main { get; set; }

        public int? visibility { get; set; }

        public Wind? wind { get; set; }

        public Clouds? clouds { get; set; }

        public int? dt { get; set; }

        public Sys? sys { get; set; }

        public int? timezone { get; set; }

        public int? id { get; set; }

        public string? name { get; set; }

        public int? cod { get; set; }

    }
}
