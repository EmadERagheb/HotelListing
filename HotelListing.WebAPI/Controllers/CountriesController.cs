using AutoMapper;
using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.County;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;

        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
        {

            _mapper = mapper;
            _countriesRepository = countriesRepository;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCoutryDTO>>> GetCountries()
        {

            List<Country> coutries = await _countriesRepository.GetAllAsync();
            var countriesDTO = _mapper.Map<List<GetCoutryDTO>>(coutries);
            return Ok(countriesDTO);

        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(q => q.Id == id, nameof(Country.Hotels));

            if (country == null)
            {
                return NotFound();
            }

            var countryDTO = _mapper.Map<CountryDTO>(country);
            return Ok(countryDTO);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDTO)
        {
            if (ModelState.IsValid)
            {
                if (id != updateCountryDTO.Id)
                {
                    return BadRequest("Invalid Id");
                }

                var country = await _countriesRepository.GetAsync(q => q.Id == id);

                _mapper.Map(updateCountryDTO, country);

                try
                {
                    await _countriesRepository.UpdateAsync(country);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CountryExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            return BadRequest(modelState: ModelState);
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO countryDTO)
        {
            //var country = new Country()
            //{
            //    Name = countryDTO.Name,
            //    ShortName = countryDTO.ShortName
            //};
            if (ModelState.IsValid)
            {
                var country = _mapper.Map<Country>(countryDTO);
              country=  await _countriesRepository.AddAsync(country);
                return CreatedAtAction("GetCountry", new { id = country.Id }, country);
            }
            else
                return BadRequest(ModelState);


        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(q=>q.Id==id);
            if (country is null)
            {
                return NotFound();
            }
            await _countriesRepository.DeleteAsync(country);
            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(q=>q.Id==id);
        }
    }
}
