using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZwalksAPI.Controllers
{
    [Route("api/[controller]")]   
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] students = new string[] { "Ranjan", "Chandan", "Kundan", "Kunal" };
            return Ok(students);
        }
    }
}
