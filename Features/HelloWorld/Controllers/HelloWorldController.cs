using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Features.Helloworld.Controllers
{
    [Route("Hello")]
    public class HelloWorldController : Controller
    {
        // GET: Hello
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Hello/Welcome
        [HttpGet("Welcome/{name?}/{numTimes?}")]
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }
    }
}