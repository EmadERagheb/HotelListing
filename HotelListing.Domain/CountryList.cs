namespace HotelListing.Domain
{
    public class CountryList
    {
        public static List<Country> Countries { get; set; } = new List<Country>()
        {
               new Country
                {
                    Id = 1,
                    Name = "Jamaica",
                    ShortName = "JM",
                    CreatedBy="Emad Ragheb",
                    UpdateBy="Emad Ragheb",
                    CreatedDate= new DateTime(2024,04,04),
                    UpdatedDate= new DateTime(2024,04,04)




                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BS",
                    CreatedBy="Emad Ragheb",
                    UpdateBy="Emad Ragheb",
                    CreatedDate= new DateTime(2024,04,04),
                    UpdatedDate= new DateTime(2024,04,04)
                },
                new Country
                {
                    Id = 3,
                    Name = "Cayman Island",
                    ShortName = "CI",
                    CreatedBy="Emad Ragheb",
                    UpdateBy="Emad Ragheb",
                    CreatedDate= new DateTime(2024,04,04),
                    UpdatedDate= new DateTime(2024,04,04)
                }
        };
    }
}
