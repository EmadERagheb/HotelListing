namespace HotelListing.Domain
{
    public class Hotel : BaseDomainModel
    {
        #region Columns
        public string Name { get; set; }
        public string Address { get; set; }

        public double Rating { get; set; }

        public int CountryId { get; set; }
        #endregion
        #region Relations
        #region Hotel-Country RS
        //Hotel Can only and Must exit at one country(one-Many)
        //required
        //column  CountryId
        //navigation Property
        public Country Country { get; set; }
        #endregion
        #endregion
    }
}
