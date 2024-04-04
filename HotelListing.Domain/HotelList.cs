namespace HotelListing.Domain
{
    public class HotelList
    {
        public static List<Hotel> Hotels { get; set; } = new List<Hotel>()
        {
             new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    CountryId = 1,
                    Rating = 4.5,
                    CreatedBy="Emad Ragheb",
                    UpdateBy="Emad Ragheb",
                    CreatedDate= new DateTime(2024,04,04),
                    UpdatedDate= new DateTime(2024,04,04)
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Comfort Suites",
                    Address = "George Town",
                    CountryId = 3,
                    Rating = 4.3,
                    CreatedBy="Emad Ragheb",
                    UpdateBy="Emad Ragheb",
                    CreatedDate= new DateTime(2024,04,04),
                    UpdatedDate= new DateTime(2024,04,04)
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Grand Palldium",
                    Address = "Nassua",
                    CountryId = 2,
                    Rating = 4,
                    CreatedBy="Emad Ragheb",
                    UpdateBy="Emad Ragheb",
                    CreatedDate= new DateTime(2024,04,04),
                    UpdatedDate= new DateTime(2024,04,04)
                }
        };
    }
}
