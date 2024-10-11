using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/")]
public class TestControllers : Controller
{
    [HttpGet]
    public async Task<IActionResult> SayHello()
    {
        return await Task.FromResult(Ok("TestMessage"));
    }
}