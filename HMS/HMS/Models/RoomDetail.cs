using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    public partial class RoomDetail
    {
        public RoomDetail()
        {
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        public int RoomId { get; set; }
        public int? HotelId { get; set; }
        [Required]
        [StringLength(20)]
        public string RoomNumber { get; set; }
        public int RoomType { get; set; }

        [ForeignKey(nameof(HotelId))]
        [InverseProperty("RoomDetails")]
        public virtual Hotel Hotel { get; set; }
        [InverseProperty(nameof(Reservation.ReservedRoom))]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
