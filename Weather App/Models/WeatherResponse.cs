using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Weather_App.Models
{
    public class WeatherResponse
    {
        public string Name { get; set; }
        public MainData Main { get; set; }
        public WindData Wind { get; set; }
        public SysData Sys { get; set; }

    }
}
