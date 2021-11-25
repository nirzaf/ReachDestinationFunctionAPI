using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachDestinationAPI.Model
{
    public class BusRoute
    {
        [Key] public int RouteNumber { get; set; }
        public int? TrueValue { get; set; }
        public int? FalseValue { get; set; }
        public string? Question { get; set; }
        public string? CurrentCity { get; set; }
    }
}
