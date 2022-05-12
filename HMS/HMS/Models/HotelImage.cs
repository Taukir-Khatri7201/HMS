using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    [Keyless]
    public partial class HotelImage
    {
        public int? HotelId { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string ImageSrc { get; set; }

        [ForeignKey(nameof(HotelId))]
        public virtual Hotel Hotel { get; set; }
    }
}
