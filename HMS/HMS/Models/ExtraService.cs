using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    public partial class ExtraService
    {
        [Key]
        public int ExtraServiceId { get; set; }
        public int? BookingId { get; set; }
        public int Type { get; set; }

        [ForeignKey(nameof(BookingId))]
        [InverseProperty(nameof(Reservation.ExtraServices))]
        public virtual Reservation Booking { get; set; }
    }
}
