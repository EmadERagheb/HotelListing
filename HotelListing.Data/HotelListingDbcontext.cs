using HotelListing.Domain;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class HotelListingDbcontext : DbContext
    {
        public HotelListingDbcontext(DbContextOptions<HotelListingDbcontext> options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveMaxLength(200);
        }
    }

}
