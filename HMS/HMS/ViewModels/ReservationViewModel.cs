using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class ReservationViewModel
    {
        public int ReservationId { get; set; }

        public int HotelId { get; set; }

        public int RoomId { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter  your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select number of rooms")]
        public int Rooms { get; set; }

        [Required(ErrorMessage = "Please select type of room")]
        public int typeOfRoom { get; set; }

        [Required(ErrorMessage = "Please select check in date")]
        [DataType(DataType.Text)]
        public DateTime CheckIn { get; set; }

        [Required(ErrorMessage = "Please select check out date")]
        [DataType(DataType.Text)]
        public DateTime CheckOut { get; set; }

        [Required(ErrorMessage = "How many adults")]
        public int Adults { get; set; }

        [Required(ErrorMessage = "How many children")]
        public int Children { get; set; }

        List<bool> ExtraServices { get; set; }

        public int FinalPrice { get; set; }
    }

    public enum ExtraServicesEnum
    {
        Birthday=1,
        Decoration,
        Anniversary,
    }
}
