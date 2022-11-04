using Microsoft.AspNetCore.Mvc;

namespace CodingChallengeAPI.Controllers
{
    //Disable caching
    [ResponseCache(NoStore = true, Duration = 0)]
    public class AppStatusController : Controller
    {
        public string Ping()
        {
            return "Hello from API";
        }

        public string AppTime()
        {
            return DateTime.Now.ToString();
        }
    }
}
