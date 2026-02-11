using Dotnet_API_10_.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_API_10_.Data
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
