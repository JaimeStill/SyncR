using Api.Services;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class ProcessController : Controller
{
    readonly ProcessorConnection processor;

    public ProcessController(ProcessorConnection processor)
    {
        this.processor = processor;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Package package)
    {
        Console.WriteLine($"Received package {package.Name} with key {package.Key}");
        bool result = await processor.SendPackage(package);
        return Ok(result);
    }
}