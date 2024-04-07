using AutoMapper;
using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.Hotal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HotelsController(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHotelDTO>>> GetHotels()
        {
            List<Hotel> hotels = await _hotelRepository.GetAllAsync();
            List<GetHotelDTO> hotelDTOs = _mapper.Map<List<GetHotelDTO>>(hotels);
            return Ok(hotelDTOs);
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<GetDetailHotel>> GetHotel(int id)
        {
            var Hotel = await _hotelRepository.GetAsync(q => q.Id == id, nameof(Domain.Hotel.Country));
            if (Hotel is not null)
            {
                var hotelDTO = _mapper.Map<GetDetailHotel>(Hotel);
                return Ok(hotelDTO);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult> PostHotel(CreateHotelDTO hotelDTO)
        {
            if (ModelState.IsValid)
            {
                var hotel = _mapper.Map<Hotel>(hotelDTO);
                hotel = await _hotelRepository.AddAsync(hotel);
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
            var hotel = await _hotelRepository.GetAsync(q => q.Id == id);
            if (hotel is null)
            {
                return NotFound();
            }
            _mapper.Map(hotelDTO, hotel);
            try
            {
                await _hotelRepository.UpdateAsync(hotel);
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
            var hotel = await _hotelRepository.GetAsync(q => q.Id == id);
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
