using Microsoft.EntityFrameworkCore;
using TestDeviceServices.Models;

namespace TestDeviceServices
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> option) : base(option) 
        {
            Database.EnsureCreated();        
        }

        public DbSet<DeviceModel> Devices { get; set; }
        public DbSet<DeviceTokens> DevicesTokens { get; set; }
        public DbSet<EventModel> Events { get; set; }
    }
}
