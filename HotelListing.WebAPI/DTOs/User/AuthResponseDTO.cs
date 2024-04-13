namespace HotelListing.WebAPI.DTOs.User
{
    public class AuthResponseDTO
    {
        public string UserId { get; set; }

        public string Tokken {  get; set; }

        public string RefreshToken { get; set; }
    }
}
