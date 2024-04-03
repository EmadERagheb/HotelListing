namespace HotelListing.Domain
{
    public abstract class BaseDomainModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdateBy { get; set; }
        public string CreatedBy { get; set; }

    }
}
