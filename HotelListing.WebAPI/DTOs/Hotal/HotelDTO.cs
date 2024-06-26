﻿using System.ComponentModel.DataAnnotations;

namespace HotelListing.WebAPI.DTOs.Hotal
{
    public class HotelDTO : HotelBaseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}

