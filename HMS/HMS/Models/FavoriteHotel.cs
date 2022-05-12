using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    [Table("FavoriteHotel")]
    public partial class FavoriteHotel
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? HotelId { get; set; }
        public bool IsFavorite { get; set; }

        [ForeignKey(nameof(HotelId))]
        [InverseProperty("FavoriteHotels")]
        public virtual Hotel Hotel { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("FavoriteHotels")]
        public virtual User User { get; set; }
    }
}
