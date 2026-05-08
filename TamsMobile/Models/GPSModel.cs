using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamsMobile.Models
{
    public class GPSModel
    {
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Address_1 { get; set; }
        public string? Address_2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }

    public class NominatimAddress
    {
        public string? building { get; set; }
        public string? road { get; set; }
        public string? neighbourhood { get; set; }
        public string? suburb { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? postcode { get; set; }
        public string? country { get; set; }
    }

    public class NominatimResponse
    {
        public NominatimAddress? address { get; set; }
    }
}
