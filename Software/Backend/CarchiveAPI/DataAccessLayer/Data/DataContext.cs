using CarchiveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarchiveAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferVehicle> OffersVehicles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehiclePhoto> VehiclePhotos { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<InvoicePenalty> InvoicesPenalties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OfferVehicle>()
                .HasKey(ov => new { ov.OfferId, ov.VehicleId });
            modelBuilder.Entity<OfferVehicle>()
                .HasOne(o => o.Offer)
                .WithMany(ov => ov.OfferVehicles)
                .HasForeignKey(v => v.VehicleId);
            modelBuilder.Entity<OfferVehicle>()
                .HasOne(v => v.Vehicle)
                .WithMany(ov => ov.OfferVehicles)
                .HasForeignKey(o => o.OfferId);

            modelBuilder.Entity<InvoicePenalty>()
                .HasKey(ip => new { ip.InvoiceId, ip.PenaltyId });
            modelBuilder.Entity<InvoicePenalty>()
                .HasOne(i => i.Invoice)
                .WithMany(ip => ip.InvoicePenalties)
                .HasForeignKey(p => p.PenaltyId);
            modelBuilder.Entity<InvoicePenalty>()
                .HasOne(p => p.Penalty)
                .WithMany(ip => ip.InvoicePenalties)
                .HasForeignKey(i => i.InvoiceId);

            modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Location)
            .WithOne(l => l.Vehicle)
            .HasForeignKey<Location>(l => l.VehicleId);

            modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Contract)
            .WithOne(c => c.Reservation)
            .HasForeignKey<Contract>(c => c.ReservationId);
        }
    }
}
