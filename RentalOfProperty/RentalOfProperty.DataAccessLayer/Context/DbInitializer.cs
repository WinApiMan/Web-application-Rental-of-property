// <copyright file="DbInitializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Context
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Class from initialize data base.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Filling the database with initial data.
        /// </summary>
        /// <param name="modelBuilder">simple API surface.</param>
        public static void Initialize(ModelBuilder modelBuilder)
        {
            // Identity roles initialize
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole[]
                {
                    new IdentityRole { Id = "1", Name = "Administrator", NormalizedName = "ADMINISTRATOR", ConcurrencyStamp = Guid.NewGuid().ToString() },
                    new IdentityRole { Id = "2", Name = "Moderator", NormalizedName = "MODERATOR", ConcurrencyStamp = Guid.NewGuid().ToString() },
                    new IdentityRole { Id = "3", Name = "User", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() },
                });

            // Create admin account
            modelBuilder.Entity<UserDTO>().HasData(
                new UserDTO
                {
                    Id = "4b6eaba4-6d87-46de-8f15-925440578cd9",
                    FullName = "Шпак Александр Cергеевич",
                    UserName = "RentalOfProperty@yandex.by",
                    PhoneNumber = "+375293182658",
                    Email = "RentalOfProperty@yandex.by",
                    NormalizedUserName = "RENTALOFPROPERTY@YANDEX.BY",
                    NormalizedEmail = "RENTALOFPROPERTY@YANDEX.BY",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEEpzp5D+Vl/3mdF1kjA7c8QjtJJfb14npeooCUJdfgkzSUH6s8uYYawwnleFCZY0Dg==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    AvatarImagePath = "~/Files/Images/DefaultAccount.png",
                });

            // Create connection between account and role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "4b6eaba4-6d87-46de-8f15-925440578cd9",
                    RoleId = "1",
                });
        }
    }
}
