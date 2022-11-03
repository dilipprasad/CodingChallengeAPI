using CodingChallenge.Business.Interfaces;
using CodingChallenge.Models;
using CodingChallenge.Models.Response;
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
        private readonly string _logTitle = "CodingChallengeAPI.Controllers.ZipLookupController";
        ICityBusinessProvider _cityBusinessProvider;
        public ZipLookupController(ILogger<ZipLookupController> logger, IMemoryCache memoryCache, ICityBusinessProvider cityBusinessProvider)
        {
            if (logger == null)
                throw new Exception("Logger object is null");

            if (memoryCache == null)
                throw new Exception("Memory Cache object is null");

            if(cityBusinessProvider==null)
                throw new Exception("City Business Provider object is null");

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
            StringBuilder sbValidation = new StringBuilder();//Using string builder incase of multiple errors
            SearchCityByZipCodeResponse response = new SearchCityByZipCodeResponse();
            try
            {
                #region Validation
                                bool hasValidationEror = false;

                                if (string.IsNullOrWhiteSpace(zipCode))
                                {
                                    _logger.LogWarning(_logTitle + " GetCityByZipCode(). Zip Code is null or empty", zipCode);
                                    hasValidationEror = true;
                                    sbValidation.AppendLine(" Zip Code is null or empty");

                                }

                                if (hasValidationEror)
                                    return BuildBadRequestActionResult(sbValidation.ToString());
                #endregion Validation

                if (_memoryCache != null && _memoryCache.Get<List<CityDetails>>(zipCode) != null)
                {//Already has data in memory
                    _logger.LogInformation(_logTitle + " Found City data for zipcode from memory.", zipCode);
                    var result = _memoryCache.Get<List<CityDetails>>(zipCode);
                    if (result != null)
                        response.CityDetails = result;

                }
                else
                {//Data is not in memory return cached response
                    _logger.LogInformation(_logTitle + " Found City data for zipcode from memory.", zipCode);
                    
                    var result = new List<CityDetails> { await _cityBusinessProvider.GetZipCodeByCity(zipCode) };

                    if (result != null)
                        _logger.LogInformation(_logTitle + "Caching data for zipcode", zipCode, result);
                        _memoryCache.Set<List<CityDetails>>(zipCode, result);
                        response.CityDetails = result;

                }
                _logger.LogInformation(_logTitle + " End of GetCityByZipCode", zipCode);
                _logger.LogTrace(_logTitle + " API Response", response);
                return BuildActionResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(_logTitle + " Error in GetCityByZipCode", zipCode, ex);
                return HandleException(_logTitle,response, ex);
            }


        }
    }
}