using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachDestinationAPI.Model
{
    public class BusStation
    {
        [Key] public int StationId { get; set; }
        public string CityName { get; set; }
    }
}
