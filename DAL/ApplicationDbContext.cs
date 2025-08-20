using Core.Enums;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DAL
{
    public class ApplicationDbContext  : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Job> Jobs { get; set; }    
        public DbSet<RepairRecord> RepairRecords { get; set; }
        public DbSet<ServiceRecord> ServiceRecords { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<DeviceImage> DeviceImages { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<RepairRecordComponent> RepairRecordComponents { get; set; }
        public DbSet<UpgradeRecord> UpgradeRecords { get; set; }
        public DbSet<UpgradeRecordComponent> UpgradeRecordComponents { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Define the relationships between the entities


            // Example: One-to-Many relationship between Customer and Device
            modelBuilder.Entity<Device>()

                .HasOne(d => d.Customer)
                .WithMany(c => c.Devices)
                .HasForeignKey(d => d.CustomerId);

            // Example: Index on Device.SerialNumber (good for searching)

            modelBuilder.Entity<Device>()
                .HasIndex(d => d.SerialNumber)
                .IsUnique(); // Optional: enforce that SerialNumbers must be unique


            // Example: Index on Device.DeviceType (good for searching)
            modelBuilder.Entity<Device>()
                .Property(d=> d.DeviceType)
                .HasConversion<string>() // Convert enum to string for storage
                .HasMaxLength(50); // Set a max length (adjust if needed for longest enum name)
                                   // e.g., "Laptop" is 6 chars, 50 gives buffer.



            // Example: One-to-Many relationship between Device and Job
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Device)
                .WithMany(d => d.Jobs)
                .HasForeignKey(j => j.DeviceId);
            // Example: Index on Job.DeviceId (good for quick lookups)
            modelBuilder.Entity<Job>()
                .HasIndex(j => j.DeviceId); // Index for quick lookups by DeviceId



            //this is removed becaue repair record does not need to have service type enum
            //modelBuilder.Entity<RepairRecord>() 
            //    .Property(r => r.ServiceType) // Target the ServiceType property
            //    .HasConversion<string>() // Tell EF Core to store it as a string
            //    .HasMaxLength(50); // Set a max length (adjust if needed for longest enum name)
            //                       // e.g., "GeneralService" is 16 chars, 50 gives buffer.


            modelBuilder.Entity<Device>()
                .HasIndex(d => d.CustomerId); // Index for quick lookups by CustomerId

            


            // NEW: Convert the RepairStatus enum to a string for storage
            modelBuilder.Entity<RepairRecord>()
                .Property(r => r.RepairStatus) // Target the Status property
                .HasConversion<string>() // Tell EF Core to store it as a string
                .HasMaxLength(50); // Set a max length (adjust if needed for longest enum name)
                                   // e.g., "ReadyForPickup" is 14 chars, 50 gives buffer.


            //one job can have one billing record

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Billing)
                .WithOne(b => b.Job)
                .HasForeignKey<Billing>(b => b.JobId);


            //one device can have multiple images
            modelBuilder.Entity<DeviceImage>()
                .HasOne(di => di.Device) // DeviceImage has one Device
                .WithMany(d => d.DeviceImages) // Device has many DeviceImages
                .HasForeignKey(di => di.DeviceId)// DeviceId is the FK in DeviceImage
                .OnDelete(DeleteBehavior.Cascade); // If a Device is deleted, delete its images too

            modelBuilder.Entity<DeviceImage>()
                .HasIndex(di => di.DeviceId); // Index for quick lookups by DeviceId`


            // one to many relationship between repair record and job

            modelBuilder.Entity<RepairRecord>()
                .HasOne(r => r.Job) // RepairRecord has one Job
                .WithMany(j => j.RepairRecords) // Job has many RepairRecords
                .HasForeignKey(r => r.JobId); // JobId is the FK in RepairRecord


            // one to many relationship between upgrade record and job
            modelBuilder.Entity<UpgradeRecord>()
                .HasOne(u => u.Job) // UpgradeRecord has one Job
                .WithMany(j => j.UpgradeRecords) // Job has many UpgradeRecords
                .HasForeignKey(u => u.JobId); // JobId is the FK in UpgradeRecord



            //one to many relationship between service record and job

            modelBuilder.Entity<ServiceRecord>()
                .HasOne(s => s.Job) // ServiceRecord has one Job
                .WithMany(j => j.ServiceRecords) // Job has many ServiceRecords
                .HasForeignKey(s => s.JobId); // JobId is the FK in ServiceRecord


            modelBuilder.Entity<RepairRecord>()
                .HasIndex(r => r.JobId); // Index for quick lookups by JobId

            modelBuilder.Entity<UpgradeRecord>()
                .HasIndex(u => u.JobId); // Index for quick lookups by JobId

            modelBuilder.Entity<ServiceRecord>()
                .HasIndex(s => s.JobId); // Index for quick lookups by JobId







            // many to many relationship between repair record and components

            modelBuilder.Entity<RepairRecordComponent>()
                            .HasKey(rc => new { rc.RepairRecordId, rc.ComponentId }); // Composite key

            // Define the relationships
            modelBuilder.Entity<RepairRecordComponent>()
                .HasOne(rc => rc.RepairRecord) // RepairRecordComponent has one RepairRecord
                .WithMany(r => r.RepairRecordComponents) // RepairRecord has many RepairRecordComponents
                .HasForeignKey(rc => rc.RepairRecordId);// RepairRecordId is the FK in RepairRecordComponent


            modelBuilder.Entity<RepairRecordComponent>()
                .HasOne(rc => rc.Component) // RepairRecordComponent has one Component
                .WithMany(c => c.RepairRecordComponents) // Component has many RepairRecordComponents
                .HasForeignKey(rc => rc.ComponentId); // ComponentId is the FK in RepairRecordComponent



            // many to many relationship between upgrade record and components
            modelBuilder.Entity<UpgradeRecordComponent>()
                .HasKey(ur => new { ur.UpgradeRecordId, ur.ComponentId }); // Composite key

            // Define the relationships
            modelBuilder.Entity<UpgradeRecordComponent>()
                .HasOne(ur => ur.UpgradeRecord) // UpgradeRecordComponent has one UpgradeRecord
                .WithMany(r => r.UpgradeRecordComponents) // UpgradeRecord has many UpgradeRecordComponents
                .HasForeignKey(ur => ur.UpgradeRecordId);// UpgradeRecordId is the FK in UpgradeRecordComponent

            modelBuilder.Entity<UpgradeRecordComponent>()
                .HasOne(ur => ur.Component) // UpgradeRecordComponent has one Component
                .WithMany(c => c.UpgradeRecordComponents) // Component has many UpgradeRecordComponents
                .HasForeignKey(ur => ur.ComponentId); // ComponentId is the FK in UpgradeRecordComponent



            modelBuilder.Entity<Component>()
                .HasIndex(c => c.SerialNumber) // Index for quick lookups by SerialNumber
                .IsUnique(); // Optional: enforce that SerialNumbers must be unique

            // Example: Index on Component.Type (good for searching)
            modelBuilder.Entity<Component>()
                .Property(c=> c.Type) // Target the Type property
                .HasConversion<string>() // Tell EF Core to store it as a string
                .HasMaxLength(50); // Set a max length (adjust if needed for longest enum name)
                                   // e.g., "GPU" is 3 chars, 50 gives buffer.

           


            modelBuilder.Entity<Billing>()
                .Property(b => b.PaymentMethod) // Target the PaymentMethod property
                .HasConversion<string>() // Tell EF Core to store it as a string
                .HasMaxLength(50); // Set a max length (adjust if needed for longest enum name)
                                   // e.g., "CreditCard" is 11 chars, 50 gives buffer.





            //newly added enums for customer and device
            modelBuilder.Entity<Customer>()
                .Property(c => c.RecordStatus)
                .HasConversion<string>()
    .           HasMaxLength(20);

            //global filters were commented out in the original code, uncomment if needed
            //modelBuilder.Entity<Customer>()
            //    .HasQueryFilter(c => c.RecordStatus == RecordStatus.Active); // Global filter

            modelBuilder.Entity<Device>()
                .Property(d => d.RecordStatus)
                .HasConversion<string>()
                .HasMaxLength(20);
            //modelBuilder.Entity<Device>()
            //    .HasQueryFilter(d => d.RecordStatus == RecordStatus.Active); // Global filter




            modelBuilder.Entity<Job>()
                .Property(j => j.JobStatus) // Target the Status property
                .HasConversion<string>() // Tell EF Core to store it as a string
                .HasMaxLength(50); // Set a max length (adjust if needed for longest enum name)
                                   // e.g., "InProgress" is 11 chars, 50 gives buffer.


            ////only show deviceimages and jobs if the parent device is active
            //modelBuilder.Entity<DeviceImage>()
            //    .HasQueryFilter(di=> di.Device != null &&  di.Device.RecordStatus == RecordStatus.Active); // Global filter

            ////only show jobs if the parent device is active
            //modelBuilder.Entity<Job>()
            //    .HasQueryFilter(j => j.Device != null && j.Device.RecordStatus == RecordStatus.Active); // Global filter





            //newly added status enums for upgrade and service records
            modelBuilder.Entity<UpgradeRecord>()
                .Property(u => u.UpgradeStatus) // Target the Status property
                .HasConversion<string>() // Tell EF Core to store it as a string
                .HasMaxLength(50); // Set a max length (adjust if needed for longest enum name)
                                   // e.g., "PendingApproval" is 15 chars, 50 gives buffer.


            modelBuilder.Entity<ServiceRecord>()
                .Property(s => s.ServiceStatus) // Target the Status property
                .HasConversion<string>() // Tell EF Core to store it as a string
                .HasMaxLength(50); // Set a max length (adjust if needed for longest enum name)
                                   // e.g., "Completed" is 9 chars, 50 gives buffer.




            //precision for decimal values

            modelBuilder.Entity<Billing>()
                .Property(b => b.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Component>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Job>()
                .Property(j => j.ManualJobCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RepairRecordComponent>()
                .Property(r => r.PricePerUnit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UpgradeRecordComponent>()
                .Property(u => u.PricePerUnit)
                .HasPrecision(18, 2);

            //modelBuilder.Entity<ServiceRecord>()
            //    .Property(s => s.Cost)
            //    .HasPrecision(18, 2);

            //modelBuilder.Entity<UpgradeRecord>()
            //    .Property(u => u.Cost)
            //    .HasPrecision(18, 2);
        }

    }
}
