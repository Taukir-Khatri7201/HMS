using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class RatingViewModel
    {
        public string UserName { get; set; }
        [Required]
        public int ReservationId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int HotelId { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Please enter your feedback")]
        public string Description { get; set; }
        public DateTime RatingDate { get; set; }
    }
}
