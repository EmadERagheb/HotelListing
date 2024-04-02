using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class HotelListingDbcontextFactory : IDesignTimeDbContextFactory<HotelListingDbcontext>
    {
        public HotelListingDbcontext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile(@".\HotelListing.WebAPI\appsettings.json")
                  .Build();
            DbContextOptionsBuilder<HotelListingDbcontext> optionsBuilder = new DbContextOptionsBuilder<HotelListingDbcontext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new HotelListingDbcontext(optionsBuilder.Options);
        }
    }
}
