using hps_api.DTOs;
using hps_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hps_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly PostgresContext _context;

        public CountryController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-country")]
        public async Task<ActionResult<ResponseDto>> GetCountrys()
        {
            var countries = await _context.Countries.OrderBy(i => i.Id).ToListAsync();

            if (countries == null || countries.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "No country list found",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "List of" + countries.Count + "infos",
                Success = true,
                Payload = new
                {
                    Output = countries
                }
            });

        }
        [HttpPost("get-country-by-id")]
        public async Task<ActionResult<ResponseDto>> GetInfos(Country country)
        {
            if (country.Id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "id error",
                    Success = false,
                    Payload = null
                });
            }

            var myCountry = await _context.Countries.Where(i => i.Id >= country.Id).FirstOrDefaultAsync();

            if (myCountry == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "No Country Found",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Country Found",
                Success = true,
                Payload = myCountry
            });
        }
        //[HttpPost]
        //public async Task<ActionResult<ResponseDto>> CreateCountry(Country myCountry)
        //{
        //    _context.Countries.Add(myCountry);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCountry", new { id = myCountry.Id }, myCountry);
        //}
        [HttpPost("create-country")]
        public async Task<ActionResult<ResponseDto>> CreateCountry(Country myCountry)
        {
            if (myCountry.CountryName == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Country name is null",
                    Success = false,
                    Payload = null
                });
            }

            Country country = await _context.Countries.Where(i => i.Id == myCountry.Id).FirstOrDefaultAsync();
            if (country != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseDto
                {
                    Message = "This country already exist",
                    Success = false,
                    Payload = null
                });
            }

            _context.Countries.Add(myCountry);
            bool isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Country creating error",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Country creating done",
                Success = true,
                Payload = new { myCountry.Id }
            });
        }
        [HttpPatch("update-countries")]
        public async Task<ActionResult<ResponseDto>> UpdateCountries(Info myCountry)
        {
            var country = _context.Countries.SingleOrDefault(x => x.Id == myCountry.Id);

            if (myCountry.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "No Countries Found",
                    Success = false,
                    Payload = null
                });
            }
            else
            {
                country.Id = myCountry.Id;
                _context.Countries.Update(country);
                var result = await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new ResponseDto
                {
                    Message = "Countries Updated",
                    Success = true,
                    Payload = new { myCountry.Id }
                });
            }

        }
        [HttpDelete("delete-countries")]
        public async Task<ActionResult<ResponseDto>> DeleteCountry(Country myCountry)
        {
            if (myCountry.Id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = " id null",
                    Success = false,
                    Payload = null
                });
            }

            Country country = await _context.Countries.Where(i => i.Id == myCountry.Id).FirstOrDefaultAsync();
            if (country == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "No country found in database",
                    Success = false,
                    Payload = null
                });
            }

            _context.Countries.Remove(country);
            bool isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "didn't delete",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "deleted",
                Success = true,
                Payload = new { myCountry.Id } // optional, can be null too like update
            });
        }
    }
}
