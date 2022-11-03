using CodingChallenge.Business.Interfaces;
using CodingChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace CodingChallengeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZipLookupController : BaseController
    {


        private readonly ILogger<ZipLookupController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly ICityDataProvider _cityDataFactory;
        private readonly string _logTitle = "CodingChallengeAPI.Controllers.ZipLookupController";

        public ZipLookupController(ILogger<ZipLookupController> logger, IMemoryCache memoryCache, ICityDataProvider cityDataProvider)
        {
            if (logger == null)
                throw new Exception("Logger object is null");

            if (memoryCache == null)
                throw new Exception("Memory Cache object is null");

            _logger = logger;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Search City By Zip Code
        /// </summary>
        /// <param name="zipCode">alphanumeric value</param>
        /// <returns>City</returns>
        [HttpGet(Name = "GetCityByZipCode")]
        //[ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> GetCityByZipCode(string zipCode)
        {
            _logger.LogInformation(_logTitle + " Begin of GetCityByZipCode", zipCode);
            StringBuilder sb = new StringBuilder();//Using string builder incase of multiple errors
            try
            {
                bool hasValidationEror = false;

                if (string.IsNullOrWhiteSpace(zipCode))
                    _logger.LogWarning(_logTitle + " GetCityByZipCode(). Zip Code is null or empty", zipCode);
                    hasValidationEror = true;
                    sb.AppendLine(" Zip Code is null or empty");


                return BuildBadRequestActionResult(sb.ToString());

            if (_memoryCache != null && _memoryCache.Get<CityDetails>(zipCode) != null)
                {

                    return BuildActionResult(_memoryCache.Get<CityDetails>(zipCode));
                }
                else
                {
                    //Caching the response
                    var cityDetails = await _cityDataFactory.GetZipCodeByCity(zipCode);
                    _memoryCache.Set<CityDetails>(zipCode, cityDetails);

                    return new CityDetails();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(_logTitle + " Error in GetCityByZipCode", zipCode,ex);
            }
           

        }
    }
}