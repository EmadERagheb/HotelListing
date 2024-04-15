namespace HotelListing.WebAPI.Models
{
    public class QueryPerimeters
    {
        private int _pageSize = 15;

        public int PageSize { get => _pageSize; set => _pageSize = value; }

        public int PageNumber { get; set; }
    }
}
