using CodingChallenge.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CodingChallengeAPI.Controllers
{
    public class BaseController : ControllerBase
    {
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
            if (response == null || (!response.IsSuccess && !response.IsException))
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
        protected IActionResult HandleException(ResponseBase response, Exception ex, String title)
        {
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
