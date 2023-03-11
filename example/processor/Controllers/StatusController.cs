using Microsoft.AspNetCore.Mvc;
using Processor.Services;

namespace Processor.Controllers;

[Route("api/[controller]")]
public class StatusController : Controller
{
    readonly ProcessorConnection svc;

    public StatusController(ProcessorConnection svc)
    {
        this.svc = svc;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Json(
                svc.Status
            );
        }
        catch
        {
            return Problem("Unable to determine SyncR service status", statusCode: 500);
        }
    }
}