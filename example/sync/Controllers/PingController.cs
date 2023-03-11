using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Sync.Controllers;

[Route("api/[controller]")]
public class PingController : Controller
{
    [HttpGet]
    public IActionResult Get() =>
        Json(
            new
            {
                Url = HttpContext.Request.GetDisplayUrl()
            }
        );
}