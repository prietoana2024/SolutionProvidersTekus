using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvidersTekus.DLL.Services;

namespace ProvidersTekus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CountriesController : ControllerBase
    {
        private readonly CountryLayerService _countryService;

        public CountriesController(CountryLayerService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("calling-code/{code}")]
        public async Task<IActionResult> GetByCallingCode(string code)
        {
            var countries = await _countryService.GetCountryByCallingCodeAsync(code);
            return Ok(countries);
        }

        [HttpGet("alpha/{code}")]
        public async Task<IActionResult> GetByAlphaCode(string code)
        {
            var country = await _countryService.GetCountryByAlphaCodeAsync(code);
            return Ok(country);
        }

    }
}
