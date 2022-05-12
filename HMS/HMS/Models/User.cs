using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    [Index(nameof(Email), Name = "UQ__Users__A9D10534FAF9CE51", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            FavoriteHotels = new HashSet<FavoriteHotel>();
            HotelReviewAndRatings = new HashSet<HotelReviewAndRating>();
            ReservationUpdatedByNavigations = new HashSet<Reservation>();
            ReservationUsers = new HashSet<Reservation>();
            StaffApplications = new HashSet<StaffApplication>();
            StaffReviewAndRatingFromUsers = new HashSet<StaffReviewAndRating>();
            StaffReviewAndRatingToUsers = new HashSet<StaffReviewAndRating>();
        }

        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Password { get; set; }
        [Required]
        [StringLength(10)]
        public string Mobile { get; set; }
        [Column("DOB", TypeName = "datetime")]
        public DateTime? Dob { get; set; }
        public short? Age { get; set; }
        public short UserType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RegisteredOn { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "text")]
        public string UserProfile { get; set; }

        [InverseProperty(nameof(FavoriteHotel.User))]
        public virtual ICollection<FavoriteHotel> FavoriteHotels { get; set; }
        [InverseProperty(nameof(HotelReviewAndRating.User))]
        public virtual ICollection<HotelReviewAndRating> HotelReviewAndRatings { get; set; }
        [InverseProperty(nameof(Reservation.UpdatedByNavigation))]
        public virtual ICollection<Reservation> ReservationUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Reservation.User))]
        public virtual ICollection<Reservation> ReservationUsers { get; set; }
        [InverseProperty(nameof(StaffApplication.User))]
        public virtual ICollection<StaffApplication> StaffApplications { get; set; }
        [InverseProperty(nameof(StaffReviewAndRating.FromUser))]
        public virtual ICollection<StaffReviewAndRating> StaffReviewAndRatingFromUsers { get; set; }
        [InverseProperty(nameof(StaffReviewAndRating.ToUser))]
        public virtual ICollection<StaffReviewAndRating> StaffReviewAndRatingToUsers { get; set; }
    }
}
