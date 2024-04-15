using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.Hotal;
using HotelListing.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;


        public HotelsController(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;

        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetHotelDTO>>> GetHotels()
        {
            List<GetHotelDTO> hotelDTOs = await _hotelRepository.GetAllAsync<GetHotelDTO>();
            return Ok(hotelDTOs);
        }
        [HttpGet]
        public async Task<ActionResult<QueryResult<GetHotelDTO>>> GetAllAsync([FromQuery] QueryPerimeters queryPrimeter)
        {
            var hotelPage = await _hotelRepository.GetAllAsync<GetHotelDTO>(queryPrimeter);
            return Ok(hotelPage);
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<GetDetailHotel>> GetHotel(int id)
        {
            var hotelDTO = await _hotelRepository.GetAsync<GetDetailHotel>(q => q.Id == id, nameof(Domain.Hotel.Country));
            if (hotelDTO is not null)
            {
                return Ok(hotelDTO);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDTO hotelDTO)
        {
            if (ModelState.IsValid)
            {

                var hotel = await _hotelRepository.AddAsync<CreateHotelDTO, Hotel>(hotelDTO);
                return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PostHotel(int id, HotelDTO hotelDTO)
        {
            if ((hotelDTO.Id != id))
            {
                return BadRequest("Invalid Id");
            }
            try
            {
                if (await _hotelRepository.UpdateAsync(id, hotelDTO) == 0)
                {
                    return NotFound();
                };
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    return NotFound();
                }
                else
                    throw;
            }
            return NoContent();


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelRepository.GetAsync<Hotel>(q => q.Id == id);
            if (hotel is null)
            {
                return NotFound();
            }
            else
                await _hotelRepository.DeleteAsync(hotel);
            return NoContent();
        }



        private async Task<bool> HotelExists(int id)
        {
            return await _hotelRepository.Exists(q => q.Id == id);
        }


    }
}
