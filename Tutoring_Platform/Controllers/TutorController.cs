using Microsoft.AspNetCore.Mvc;

namespace Tutoring_Platform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TutorController : ControllerBase
    {
        
        [HttpGet]
        public string Get()
        {
            Console.WriteLine("Got it");
            var allTrips = "ALL GOOD";
            return (allTrips);
        }
    }
}
