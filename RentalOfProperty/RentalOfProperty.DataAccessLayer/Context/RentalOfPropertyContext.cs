// <copyright file="RentalOfPropertyContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Context
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Data base context.
    /// </summary>
    public class RentalOfPropertyContext : IdentityDbContext<UserDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentalOfPropertyContext"/> class.
        /// </summary>
        /// <param name="options">Options for data base settings.</param>
        public RentalOfPropertyContext(DbContextOptions<RentalOfPropertyContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Gets or sets aditional ad data list.
        /// </summary>
        public DbSet<AditionalAdDataDTO> AditionalAdDatas { get; set; }

        /// <summary>
        /// Gets or sets contact persons list.
        /// </summary>
        public DbSet<ContactPersonDTO> ContactPersons { get; set; }

        /// <summary>
        /// Gets or sets housing photos list.
        /// </summary>
        public DbSet<HousingPhotoDTO> HousingPhotos { get; set; }

        /// <summary>
        /// Gets or sets rental ads list.
        /// </summary>
        public DbSet<RentalAdDTO> RentalAds { get; set; }

        /// <summary>
        /// Gets or sets daily rental ads list.
        /// </summary>
        public DbSet<DailyRentalAdDTO> DailyRentalAds { get; set; }

        /// <summary>
        /// Gets or sets long term rental ads list.
        /// </summary>
        public DbSet<LongTermRentalAdDTO> LongTermRentalAds { get; set; }

        /// <summary>
        /// Method for setting parameters when creating a database.
        /// </summary>
        /// <param name="modelBuilder">Model builder object.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create connections
            modelBuilder.Entity<HousingPhotoDTO>()
                .HasOne<RentalAdDTO>()
                .WithMany()
                .HasForeignKey(housingPhoto => housingPhoto.RentalAdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RentalAdDTO>()
               .HasOne<ContactPersonDTO>()
               .WithMany()
               .HasForeignKey(rentalAd => rentalAd.ContactPersonId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AditionalAdDataDTO>()
                .HasOne<RentalAdDTO>()
                .WithOne()
                .HasForeignKey<AditionalAdDataDTO>(aditionalAdData => aditionalAdData.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Initialize data base
            DbInitializer.Initialize(modelBuilder);
        }
    }
}
