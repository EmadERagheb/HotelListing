namespace HotelListing.WebAPI.Models
{
    public class QueryResult<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int RecordNumber { get; set; }
        public List<T> Items { get; set; }

    }
}
