using Asp.Versioning;
using AutoMapper;
using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.County;
using HotelListing.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.WebAPI.Controllers
{
    //[Route("api/v{version:apiVersion}/Countries")]
    //[ApiController]
    //[ApiVersion("1.0",Deprecated =true)]
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {

       
        private readonly ICountriesRepository _countriesRepository;

        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
        {

     
            _countriesRepository = countriesRepository;
        }

        // GET: api/Countries
        [HttpGet("GetAll")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<GetCoutryDTO>>> GetCountries()
        {

            List<GetCoutryDTO> countriesDTO = await _countriesRepository.GetAllAsync<GetCoutryDTO>();
            return Ok(countriesDTO);
        }    
        [HttpGet]
        // GET: api/Countries/?PageNumber=5%PageSize=3
        public async Task<ActionResult<QueryResult<GetCoutryDTO>>> GetPagedCountries([FromQuery] QueryPerimeters query)
        {
           return await _countriesRepository.GetAllAsync<GetCoutryDTO>(query);

        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            var countryDTO = await _countriesRepository.GetAsync<CountryDTO>(q => q.Id == id, nameof(Country.Hotels));

            if (countryDTO == null)
            {
                return NotFound();
            }

            return Ok(countryDTO);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDTO)
        {
            if (ModelState.IsValid)
            {
                if (id != updateCountryDTO.Id)
                {
                    return BadRequest("Invalid Id");
                }
                try
                {
                    if (await _countriesRepository.UpdateAsync(id, updateCountryDTO) == 0)
                        return NotFound();
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
        //[Authorize]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO countryDTO)
        {
            //var country = new Country()
            //{
            //    Name = countryDTO.Name,
            //    ShortName = countryDTO.ShortName
            //};
            if (ModelState.IsValid)
            {
               
            var  country=  await _countriesRepository.AddAsync<CreateCountryDTO, Country>(countryDTO);
                return CreatedAtAction("GetCountry", new { id = country.Id }, country);
            }
            else
                return BadRequest(ModelState);


        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync<Country>(q=>q.Id==id);
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
