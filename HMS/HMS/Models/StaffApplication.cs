using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    public partial class StaffApplication
    {
        [Key]
        public int ApplicationId { get; set; }
        public int? UserId { get; set; }
        public int? HotelId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ApplicationDate { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string ApplicationText { get; set; }
        public int ApplicationType { get; set; }
        public bool IsApproved { get; set; }

        [ForeignKey(nameof(HotelId))]
        [InverseProperty("StaffApplications")]
        public virtual Hotel Hotel { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("StaffApplications")]
        public virtual User User { get; set; }
    }
}
