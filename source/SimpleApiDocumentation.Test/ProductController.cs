using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleApiDocumentation.Test;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok("test");
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok("test");
    }

    [HttpPost]
    public IActionResult Create()
    {
        return Ok("test");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok("test");
    }

    [HttpPut("{id}/{newName}")]
    public IActionResult Update(int id, string newName)
    {
        return Ok("test");
    }
}