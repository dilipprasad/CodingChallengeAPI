using Microsoft.AspNetCore.Mvc;

namespace CodingChallengeAPI.Controllers
{
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
