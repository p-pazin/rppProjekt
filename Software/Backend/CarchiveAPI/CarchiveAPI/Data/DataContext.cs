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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OfferVehicle>()
                .HasKey(ov => new {ov.IdOffer, ov.IdVehicle});
            modelBuilder.Entity<OfferVehicle>()
                .HasOne(o => o.Offer)
                .WithMany(ov => ov.OfferVehicles)
                .HasForeignKey(v => v.IdVehicle);
            modelBuilder.Entity<OfferVehicle>()
                .HasOne(v => v.Vehicle)
                .WithMany(ov => ov.OfferVehicles)
                .HasForeignKey(o => o.IdOffer);

            modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Ad)
            .WithOne(a => a.Vehicle)
            .HasForeignKey<Ad>(a => a.IdVehicle);
            modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Location)
            .WithOne(l => l.Vehicle)
            .HasForeignKey<Location>(l => l.IdVehicle);
            modelBuilder.Entity<Contract>()
            .HasOne(c => c.Invoice)
            .WithOne(i => i.Contract)
            .HasForeignKey<Invoice>(i => i.IdContract);
            modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithOne(c => c.Director)
            .HasForeignKey<Company>(c => c.IdDirector);
        }
    }
}
