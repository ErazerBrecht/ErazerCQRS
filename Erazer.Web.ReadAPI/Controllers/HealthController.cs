using System;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.ReadAPI.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        // GET
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Health = "OK",
                Uptime = new TimeSpan(88, 0, 0),
                DatabaseHealth = "OK",
                ServiceBusHealth = "OK",
                TODO = true
            });
        }
    }
}

