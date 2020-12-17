using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalOfProperty.DataAccessLayer.Models;

namespace RentalOfProperty.DataAccessLayer.Context
{
    public class RentalOfPropertyContext : IdentityDbContext<UserDTO>
    {
        public RentalOfPropertyContext(DbContextOptions<RentalOfPropertyContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
