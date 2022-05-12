using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    [Keyless]
    public partial class RoomReservationTiming
    {
        public int? RoomId { get; set; }
        public int? BookingId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CheckInDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CheckOutDate { get; set; }

        [ForeignKey(nameof(BookingId))]
        public virtual Reservation Booking { get; set; }
        [ForeignKey(nameof(RoomId))]
        public virtual RoomDetail Room { get; set; }
    }
}
