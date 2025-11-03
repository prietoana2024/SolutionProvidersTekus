using ProvidersTekus.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DLL.Services
{
    public class CountryLayerService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "692988f2d0879e02eb41446fde0e5400";
        private const string BaseUrl = "https://api.countrylayer.com/v2";

        public CountryLayerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener todos los países
        public async Task<List<CountryDTO>?> GetAllCountriesAsync()
        {
            var url = $"{BaseUrl}/all?access_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<List<CountryDTO>>(url);
        }

        // Buscar país por nombre
        public async Task<List<CountryDTO>?> GetCountryByNameAsync(string name, bool fullText = false)
        {
            var url = $"{BaseUrl}/name/{name}?access_key={_apiKey}&fulltext={fullText.ToString().ToLower()}";
            return await _httpClient.GetFromJsonAsync<List<CountryDTO>>(url);
        }

        // Buscar país por capital
        public async Task<List<CountryDTO>?> GetCountryByCapitalAsync(string capital)
        {
            var url = $"{BaseUrl}/capital/{capital}?access_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<List<CountryDTO>>(url);
        }

        // Buscar países por código de llamada (número)
        public async Task<List<CountryDTO>?> GetCountryByCallingCodeAsync(string callingCode)
        {
            var url = $"{BaseUrl}/callingcode/{callingCode}?access_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<List<CountryDTO>>(url);
        }

        // Buscar país por código alfa
        public async Task<CountryDTO?> GetCountryByAlphaCodeAsync(string code)
        {
            var url = $"{BaseUrl}/alpha/{code}?access_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<CountryDTO>(url);
        }

        // Buscar países por moneda
        public async Task<List<CountryDTO>?> GetCountriesByCurrencyAsync(string currency)
        {
            var url = $"{BaseUrl}/currency/{currency}?access_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<List<CountryDTO>>(url);
        }

        // Buscar países por región
        public async Task<List<CountryDTO>?> GetCountriesByRegionAsync(string region)
        {
            var url = $"{BaseUrl}/region/{region}?access_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<List<CountryDTO>>(url);
        }

        // Buscar países por idioma
        public async Task<List<CountryDTO>?> GetCountriesByLanguageAsync(string language)
        {
            var url = $"{BaseUrl}/language/{language}?access_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<List<CountryDTO>>(url);
        }


    }
}
