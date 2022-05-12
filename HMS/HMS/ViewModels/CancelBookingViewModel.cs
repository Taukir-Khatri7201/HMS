using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class CancelBookingViewModel
    {
        [Required]
        public int BookingId { get; set; }
        [Required(ErrorMessage = "Please provide reason for cancellation")]
        public string Reason { get; set; }
    }
}
