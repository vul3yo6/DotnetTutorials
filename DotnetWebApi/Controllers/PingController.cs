using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotnetWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        // GET: api/<PingController>
        [HttpGet]
        public string Get()
        {
            return $"timetag: {DateTime.Now:yyyy/MM/dd HH:mm:ss.fffffff}";
        }
    }
}
