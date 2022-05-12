using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    [Table("Hotel")]
    public partial class Hotel
    {
        public Hotel()
        {
            FavoriteHotels = new HashSet<FavoriteHotel>();
            HotelReviewAndRatings = new HashSet<HotelReviewAndRating>();
            Reservations = new HashSet<Reservation>();
            RoomDetails = new HashSet<RoomDetail>();
            StaffApplications = new HashSet<StaffApplication>();
            StaffReviewAndRatings = new HashSet<StaffReviewAndRating>();
        }

        [NotMapped]
        public string ImgSrc { get; set; }

        [Key]
        public int HotelId { get; set; }
        [Required]
        [StringLength(30)]
        public string HotelName { get; set; }
        [Required]
        [StringLength(30)]
        public string AreaName { get; set; }
        [Required]
        [StringLength(10)]
        public string ContactNumber { get; set; }
        [Required]
        [StringLength(30)]
        public string City { get; set; }
        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }
        public double? HotelRating { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        public long? Price { get; set; }
        public int? Discount { get; set; }
        public long? ActualPrice { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string SpecialFeatures { get; set; }
        [Column(TypeName = "text")]
        public string LocationAndTransport { get; set; }

        [InverseProperty(nameof(FavoriteHotel.Hotel))]
        public virtual ICollection<FavoriteHotel> FavoriteHotels { get; set; }
        [InverseProperty(nameof(HotelReviewAndRating.Hotel))]
        public virtual ICollection<HotelReviewAndRating> HotelReviewAndRatings { get; set; }
        [InverseProperty(nameof(Reservation.Hotel))]
        public virtual ICollection<Reservation> Reservations { get; set; }
        [InverseProperty(nameof(RoomDetail.Hotel))]
        public virtual ICollection<RoomDetail> RoomDetails { get; set; }
        [InverseProperty(nameof(StaffApplication.Hotel))]
        public virtual ICollection<StaffApplication> StaffApplications { get; set; }
        [InverseProperty(nameof(StaffReviewAndRating.Hotel))]
        public virtual ICollection<StaffReviewAndRating> StaffReviewAndRatings { get; set; }
    }
}
