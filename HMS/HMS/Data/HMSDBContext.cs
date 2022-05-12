using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HMS.Models;

#nullable disable

namespace HMS.Data
{
    public partial class HMSDBContext : DbContext
    {
        public HMSDBContext()
        {
        }

        public HMSDBContext(DbContextOptions<HMSDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExtraService> ExtraServices { get; set; }
        public virtual DbSet<FavoriteHotel> FavoriteHotels { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelImage> HotelImages { get; set; }
        public virtual DbSet<HotelReviewAndRating> HotelReviewAndRatings { get; set; }
        public virtual DbSet<HotelRoom> HotelRooms { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<RoomDetail> RoomDetails { get; set; }
        public virtual DbSet<RoomReservationTiming> RoomReservationTimings { get; set; }
        public virtual DbSet<StaffApplication> StaffApplications { get; set; }
        public virtual DbSet<StaffReviewAndRating> StaffReviewAndRatings { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;initial catalog=HMSDB;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExtraService>(entity =>
            {
                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.ExtraServices)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__ExtraServ__Booki__7A672E12");
            });

            modelBuilder.Entity<FavoriteHotel>(entity =>
            {
                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.FavoriteHotels)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__FavoriteH__Hotel__17036CC0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteHotels)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__FavoriteH__UserI__160F4887");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.Property(e => e.HotelRating).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<HotelImage>(entity =>
            {
                entity.HasOne(d => d.Hotel)
                    .WithMany()
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelImag__Hotel__5CD6CB2B");
            });

            modelBuilder.Entity<HotelReviewAndRating>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("PK__HotelRev__74BC79CEBFFDB57E");

                entity.Property(e => e.RatedOn).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelReviewAndRatings)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelRevi__Hotel__5FB337D6");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.HotelReviewAndRatings)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__HotelRevi__Reser__2739D489");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelReviewAndRatings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__HotelRevi__UserI__60A75C0F");
            });

            modelBuilder.Entity<HotelRoom>(entity =>
            {
                entity.HasOne(d => d.Hotel)
                    .WithMany()
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelRoom__Hotel__6383C8BA");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.BookingId)
                    .HasName("PK__Reservat__73951AED401C8ACB");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NumberOfGuests).HasDefaultValueSql("((1))");

                entity.Property(e => e.NumberOfRooms).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__Reservati__Hotel__72C60C4A");

                entity.HasOne(d => d.ReservedRoom)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ReservedRoomId)
                    .HasConstraintName("FK__Reservati__Reser__75A278F5");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.ReservationUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__Reservati__Updat__778AC167");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReservationUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Reservati__UserI__71D1E811");
            });

            modelBuilder.Entity<RoomDetail>(entity =>
            {
                entity.HasKey(e => e.RoomId)
                    .HasName("PK__RoomDeta__3286393932054DAF");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.RoomDetails)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__RoomDetai__Hotel__6EF57B66");
            });

            modelBuilder.Entity<RoomReservationTiming>(entity =>
            {
                entity.HasOne(d => d.Booking)
                    .WithMany()
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__RoomReser__Booki__7D439ABD");

                entity.HasOne(d => d.Room)
                    .WithMany()
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__RoomReser__RoomI__7C4F7684");
            });

            modelBuilder.Entity<StaffApplication>(entity =>
            {
                entity.HasKey(e => e.ApplicationId)
                    .HasName("PK__StaffApp__C93A4C998D951276");

                entity.Property(e => e.ApplicationDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.StaffApplications)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__StaffAppl__Hotel__03F0984C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StaffApplications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__StaffAppl__UserI__02FC7413");
            });

            modelBuilder.Entity<StaffReviewAndRating>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("PK__StaffRev__74BC79CE37720A70");

                entity.Property(e => e.RatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ratings).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.StaffReviewAndRatings)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__StaffRevi__Booki__0C85DE4D");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.StaffReviewAndRatingFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK__StaffRevi__FromU__07C12930");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.StaffReviewAndRatings)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__StaffRevi__Hotel__0B91BA14");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.StaffReviewAndRatingToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK__StaffRevi__ToUse__08B54D69");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.RegisteredOn).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
