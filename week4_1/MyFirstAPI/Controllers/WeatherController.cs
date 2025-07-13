using Microsoft.AspNetCore.Mvc;

namespace MyFirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private static List<string> cities = new List<string> 
        { 
            "Delhi", "Mumbai", "Bangalore", "Chennai" 
        };

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCities()
        {
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetCity(int id)
        {
            if (id < 0 || id >= cities.Count)
            {
                return NotFound("City नहीं मिली");
            }
            return Ok(cities[id]);
        }

        [HttpPost]
        public ActionResult AddCity([FromBody] string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                return BadRequest("City name खाली नहीं हो सकती");
            }
            
            cities.Add(cityName);
            return Ok($"City '{cityName}' successfully add हो गई!");
        }
    }
}