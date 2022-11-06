using CodingChallenge.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace CodingChallengeAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        protected MemoryCacheEntryOptions MemoryCacheOption { get; private set; }

        public BaseController(IConfiguration config)
        {
            var serverMemCacheDuration = Convert.ToInt32(config["Values:ServerMemoryCacheDurationInSeconds"]);
            MemoryCacheOption = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(serverMemCacheDuration));//Using sliding expiration to keep in memory most recently accessed items

        }

        /// <summary>
        /// Use this if the Request object has issues like missing request parameters
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected BadRequestObjectResult BuildBadRequestActionResult(string message)
        {
            return BadRequest(message);
        }

        /// <summary>
        /// Use this for return all the valid response 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected IActionResult BuildActionResult(ResponseBase response)
        {
            IActionResult result;
            if (response == null || response.IsException)
            {
                result = Problem();
            }
            else //Populate common response items
            {
                response.Version = "v1";
                result = Ok(response);

            }
            return result;
        }

        /// <summary>
        /// Use this for wrapping with error details
        /// </summary>
        /// <param name="response"></param>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected IActionResult HandleException(String title, ResponseBase response, Exception ex)
        {
            if (response == null)
                response = new ResponseBase();
            response.IsSuccess = ResponseStatus.FAILED;

            if (ex is Exception)
            {
                var message = "Title: " + title + "Message: ";
                //#if DEBUG
                message += ex.Message + "\n " + ex.InnerException?.Message + "\n " + ex.StackTrace;
                //#else
                //                message += "Problem in Execution";
                //#endif
                response.IsException = true;
                response.ErrorDetails = new List<Error>{
                    new Error()
                {
                    Code = "CommonErrorCodes.INTERNAL_FAILURE",
                    Message = message,
                    Logged = true
                }};
            }

            return BuildActionResult(response);
        }
    }
}
