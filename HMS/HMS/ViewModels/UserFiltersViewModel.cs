using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class UserFiltersViewModel
    {
        public string location { get; set; }
        [DataType(DataType.Text)]
        public DateTime checkIn { get; set; }
        [DataType(DataType.Text)]
        public DateTime checkOut { get; set; }
        public int adults { get; set; }
        public int children { get; set; }
        public int rooms { get; set; }
        public int typeOfRoom { get; set; }
        public bool fivestar { get; set; }
        public bool fourstar { get; set; }
        public bool threestar { get; set; }
        public bool twostar { get; set; }
        public bool onestar { get; set; }

        public bool price1 { get; set; }
        public bool price2 { get; set; }
        public bool price3 { get; set; }
        public bool price4 { get; set; }
        public bool price5 { get; set; }
    }
}
