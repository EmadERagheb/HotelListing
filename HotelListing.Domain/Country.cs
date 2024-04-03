namespace HotelListing.Domain
{
    public class Country : BaseDomainModel
    {
        public string Name { get; set; }
        public string   ShortName { get; set; }
        #region Relations
        #region Hotel-Country RS
        //Country May Have Many Hotels(one-Many)
        //optional
        //column  not exist
        //navigation Property
        public List<Hotel> Hotels { get; set; } = new List<Hotel>();

        #endregion
        #endregion
    }
}
