using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Sample_localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        public HomeController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        [HttpPost]
        public IActionResult Post()
        {
            var x = _localizer["Hello"].Value;
            return Ok(x);
        }
    }
}
