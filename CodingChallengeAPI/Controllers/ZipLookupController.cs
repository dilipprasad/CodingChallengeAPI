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


        private readonly CodingChallenge.Logging.Interface.ILogging<ZipLookupController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly string _logTitle = "CodingChallengeAPI.Controllers.ZipLookupController";
        private readonly ICityBusinessProvider _cityBusinessProvider = null;

        public ZipLookupController(CodingChallenge.Logging.Interface.ILogging<ZipLookupController> logger, IMemoryCache memoryCache, ICityBusinessProvider cityBusinessProvider, IConfiguration config) : base(config)
        {
            if (logger == null)
                throw new Exception("Logger object is null");

            if (memoryCache == null)
                throw new Exception("Memory Cache object is null");

            if (cityBusinessProvider == null)
                throw new Exception("City Business Provider object is null");

            _logger = logger;
            _memoryCache = memoryCache;
            _cityBusinessProvider = cityBusinessProvider;
        }

        /// <summary>
        /// Search City By Zip Code
        /// </summary>
        /// <param name="zipCode">alphanumeric value</param>
        /// <returns>City</returns>
        [HttpGet(Name = "GetCityByZipCode")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "zipCode" })]
        public async Task<IActionResult> GetCityByZipCode(string zipCode)
        {
            _logger.LogInfo(_logTitle + " Begin of GetCityByZipCode", new[] { zipCode });
            StringBuilder sbValidation = new StringBuilder();//Using string builder incase of multiple errors
            SearchCityByZipCodeResponse response = new SearchCityByZipCodeResponse();
            try
            {
                #region Validation
                bool hasValidationEror = false;

                if (string.IsNullOrWhiteSpace(zipCode))
                {
                    _logger.LogWarning(_logTitle + " GetCityByZipCode(). Zip Code is null or empty", new[] { zipCode });
                    hasValidationEror = true;
                    sbValidation.AppendLine(" Zip Code is null or empty");

                }

                if (hasValidationEror)
                    return BuildBadRequestActionResult(sbValidation.ToString());
                #endregion Validation

                List<CityDetails> result = null;

                if (!_memoryCache.TryGetValue<List<CityDetails>>(zipCode, out result))
                {
                    //Data is not in memory return cached response
                    _logger.LogInfo(_logTitle + " Did not find City data for zipcode from memory.", new[] { zipCode });
                    result = await _cityBusinessProvider.GetZipCodeByCity(zipCode);
                    //Now caching data
                    _logger.LogInfo(_logTitle + "Caching data for zipcode", new[] { zipCode, result as object });
                    _memoryCache.Set<List<CityDetails>>(zipCode, result, MemoryCacheOption);
                }
                else
                {
                    _logger.LogInfo(_logTitle + " Returning City data for zipcode from memory cache.", new[] { zipCode });

                }
                if (result != null)
                    response.CityDetails = result;
                response.IsSuccess = true;

                _logger.LogInfo(_logTitle + " End of GetCityByZipCode", new[] { zipCode });
                _logger.LogTrace(_logTitle + " API Response", new[] { response });
                return BuildActionResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(_logTitle + " Error in GetCityByZipCode", new[] { zipCode, ex as object });
                return HandleException(_logTitle, response, ex);
            }


        }
    }
}