using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    [Table("HotelReviewAndRating")]
    public partial class HotelReviewAndRating
    {
        [Key]
        public int ReviewId { get; set; }
        public int? HotelId { get; set; }
        public int? UserId { get; set; }
        public double Ratings { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RatedOn { get; set; }
        [Column(TypeName = "text")]
        public string Feedback { get; set; }
        public int? ReservationId { get; set; }

        [ForeignKey(nameof(HotelId))]
        [InverseProperty("HotelReviewAndRatings")]
        public virtual Hotel Hotel { get; set; }
        [ForeignKey(nameof(ReservationId))]
        [InverseProperty("HotelReviewAndRatings")]
        public virtual Reservation Reservation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("HotelReviewAndRatings")]
        public virtual User User { get; set; }
    }
}
