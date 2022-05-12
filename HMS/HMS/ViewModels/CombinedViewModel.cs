using HMS.Models;
using HMS.ViewModels;
using System.Collections.Generic;

namespace HMS.ViewModels
{
    public class CombinedViewModel
    {
        public LoginViewModel LoginModel { get; set; }
        public ForgotPasswordViewModel forgotPasswordViewModel { get; set; }
        public UserFiltersViewModel UserFilters { get; set; }
        public UserFiltersViewModel UserFilters2 { get; set; }
        public List<Hotel> hotels { get; set; }
        public HotelDetailsViewModel hotelDetails { get; set; }
        public FavHotelViewModel favHotel { get; set; }
        public List<BookingHistoryViewModel> bookingHistory { get; set; }
        public List<BookingHistoryHelperViewModel> bookingHistoryHelper { get; set; }

        public ReservationViewModel reservation { get; set; }

        public ContactViewModel ContactViewModel { get; set; }
        
        public RatingViewModel ratingViewModel { get; set; }
        
        public CancelBookingViewModel cancel { get; set; }
    }
}
