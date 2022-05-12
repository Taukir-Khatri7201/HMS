using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HMS.Models
{
    [Keyless]
    public partial class HotelRoom
    {
        public int? HotelId { get; set; }
        [Column("No_Total")]
        public int NoTotal { get; set; }
        [Column("No_AC_Single")]
        public int NoAcSingle { get; set; }
        [Column("No_AC_Double")]
        public int NoAcDouble { get; set; }
        [Column("No_AC_Triple")]
        public int NoAcTriple { get; set; }
        [Column("No_Non_AC_Single")]
        public int NoNonAcSingle { get; set; }
        [Column("No_Non_AC_Double")]
        public int NoNonAcDouble { get; set; }
        [Column("No_Non_AC_Triple")]
        public int NoNonAcTriple { get; set; }
        [Column("No_King")]
        public int NoKing { get; set; }
        [Column("No_Queen")]
        public int NoQueen { get; set; }

        [ForeignKey(nameof(HotelId))]
        public virtual Hotel Hotel { get; set; }
    }
}
