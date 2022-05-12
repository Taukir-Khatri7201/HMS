using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    [Table("StaffReviewAndRating")]
    public partial class StaffReviewAndRating
    {
        [Key]
        public int ReviewId { get; set; }
        public int? FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public double? Ratings { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RatedOn { get; set; }
        public int? HotelId { get; set; }
        public int? BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        [InverseProperty(nameof(Reservation.StaffReviewAndRatings))]
        public virtual Reservation Booking { get; set; }
        [ForeignKey(nameof(FromUserId))]
        [InverseProperty(nameof(User.StaffReviewAndRatingFromUsers))]
        public virtual User FromUser { get; set; }
        [ForeignKey(nameof(HotelId))]
        [InverseProperty("StaffReviewAndRatings")]
        public virtual Hotel Hotel { get; set; }
        [ForeignKey(nameof(ToUserId))]
        [InverseProperty(nameof(User.StaffReviewAndRatingToUsers))]
        public virtual User ToUser { get; set; }
    }
}
