using HMS.Models;
using System;

namespace HMS.ViewModels
{
    public class BookingHistoryViewModel
    {
        public int BookingId { get; set; }
        public Hotel hotel { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int TotalAmount { get; set; }
        public int Status { get; set; }
        public int TotalNights { get; set; }
        public int TotalGuests { get; set; }
        public int RoomType { get; set; }
    }
}
