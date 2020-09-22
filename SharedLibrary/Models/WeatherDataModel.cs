using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class WeatherDataModel
    {
        public Current current { get; set; }
    }
    public class Current
    {
        public int temperature { get; set; }
        public int humidity { get; set; }
    }
}

