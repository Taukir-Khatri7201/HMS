using HMS.Models;
using System.Collections.Generic;

namespace HMS.ViewModels
{
    public class HotelDetailsViewModel
    {
        public Hotel hotelDetails { get; set; }
        public List<string> images { get; set; }
        public string bannerImage { get; set; }
    }
}
