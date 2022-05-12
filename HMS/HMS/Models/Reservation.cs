using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            ExtraServices = new HashSet<ExtraService>();
            HotelReviewAndRatings = new HashSet<HotelReviewAndRating>();
            StaffReviewAndRatings = new HashSet<StaffReviewAndRating>();
        }

        [Key]
        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public int? HotelId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CheckInDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int NumberOfRooms { get; set; }
        public int RoomType { get; set; }
        public double TotalAmount { get; set; }
        public int Status { get; set; }
        public int? ReservedRoomId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

        [ForeignKey(nameof(HotelId))]
        [InverseProperty("Reservations")]
        public virtual Hotel Hotel { get; set; }
        [ForeignKey(nameof(ReservedRoomId))]
        [InverseProperty(nameof(RoomDetail.Reservations))]
        public virtual RoomDetail ReservedRoom { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("ReservationUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("ReservationUsers")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(ExtraService.Booking))]
        public virtual ICollection<ExtraService> ExtraServices { get; set; }
        [InverseProperty(nameof(HotelReviewAndRating.Reservation))]
        public virtual ICollection<HotelReviewAndRating> HotelReviewAndRatings { get; set; }
        [InverseProperty(nameof(StaffReviewAndRating.Booking))]
        public virtual ICollection<StaffReviewAndRating> StaffReviewAndRatings { get; set; }
    }
}
