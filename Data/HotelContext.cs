using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using otel_advisor_webApp.Models;

namespace otel_advisor_webApp.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {
        }

        public DbSet<Hotel> Def_Hotel { get; set; }
        public DbSet<Experience> Inf_Experience { get; set; }
        public DbSet<HotelExperience> Rel_HotelExperience { get; set; }
        public DbSet<ReservationRequest> Inf_Reservation { get; set; }
        public DbSet<User> Def_User { get; set; }
        public DbSet<UserPreference> Rel_UserPreference { get; set; }
        public DbSet<Location> Def_Location { get; set; }
        public DbSet<ReservationOffer> Inf_ReservationOffer { get; set; } 
        public DbSet<ReservationConfirmed> ReservationConfirmed { get; set; }
        public DbSet<UserHotelExperience> UserHotelExperiences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define Table Names
            modelBuilder.Entity<Hotel>().ToTable("def_hotel");
            modelBuilder.Entity<Experience>().ToTable("def_experience");
            modelBuilder.Entity<HotelExperience>().ToTable("rel_hotel_experience");
            modelBuilder.Entity<ReservationRequest>().ToTable("inf_reservation_request");
            modelBuilder.Entity<User>().ToTable("def_user");
            modelBuilder.Entity<UserPreference>().ToTable("rel_user_experience");
            modelBuilder.Entity<Location>().ToTable("ref_location");
            modelBuilder.Entity<ReservationOffer>().ToTable("inf_reservation_offer");
            modelBuilder.Entity<ReservationConfirmed>().ToTable("inf_reservation_confirmed");
            modelBuilder.Entity<UserHotelExperience>().ToTable("inf_user_hotel_experience");

            modelBuilder.Entity<UserHotelExperience>()
                .HasKey(uhe => uhe.user_hotel_experience_id);


            modelBuilder.Entity<Hotel>()
                .HasKey(h => h.hotel_id);

            modelBuilder.Entity<Experience>()
                .HasKey(e => e.experience_id);

            modelBuilder.Entity<HotelExperience>()
                .HasKey(he => new { he.hotel_id, he.experience_id });

            modelBuilder.Entity<ReservationRequest>()
                .HasKey(r => r.reservation_request_id);

            modelBuilder.Entity<User>()
                .HasKey(u => u.user_id);

            modelBuilder.Entity<UserPreference>()
                .HasKey(up => up.user_preference_id);

            modelBuilder.Entity<Location>()
                .HasKey(l => l.location_id);

            modelBuilder.Entity<ReservationConfirmed>()
                .HasKey(rc => rc.confirmed_reservation_id);

            modelBuilder.Entity<ReservationRequest>()
            .Property(r => r.children_ages)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<int>>(v));

            modelBuilder.Entity<HotelExperience>()
                .HasOne(he => he.hotel)
                .WithMany(h => h.HotelExperiences)
                .HasForeignKey(he => he.hotel_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HotelExperience>()
                .HasOne(he => he.experience)
                .WithMany(e => e.HotelExperiences)
                .HasForeignKey(he => he.experience_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservationRequest>()
                .HasOne(r => r.user)
                .WithMany(u => u.ReservationRequests)
                .HasForeignKey(r => r.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPreference>()
                .HasOne(up => up.user)
                .WithMany(u => u.UserPreferences)
                .HasForeignKey(up => up.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPreference>()
                .HasOne(up => up.experience)
                .WithMany(e => e.UserPreferences)
                .HasForeignKey(up => up.experience_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.Location)
                .WithMany(l => l.Hotels)
                .HasForeignKey(h => h.location_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservationConfirmed>()
                .HasOne(rc => rc.ReservationRequest)
                .WithMany()
                .HasForeignKey(rc => rc.reservation_request_id);

            modelBuilder.Entity<UserHotelExperience>()
                .HasOne(uhe => uhe.User)
                .WithMany()
                .HasForeignKey(uhe => uhe.user_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserHotelExperience>()
                .HasOne(uhe => uhe.Hotel)
                .WithMany()
                .HasForeignKey(uhe => uhe.hotel_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserHotelExperience>()
                .HasOne(uhe => uhe.ReservationConfirmed)
                .WithMany()
                .HasForeignKey(uhe => uhe.reservation_request_id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
