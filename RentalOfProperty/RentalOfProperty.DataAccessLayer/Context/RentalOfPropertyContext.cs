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
        /// Method for setting parameters when creating a database.
        /// </summary>
        /// <param name="modelBuilder">Model builder object.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Initialize data base
            DbInitializer.Initialize(modelBuilder);
        }
    }
}
