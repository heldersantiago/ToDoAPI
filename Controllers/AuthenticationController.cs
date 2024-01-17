using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApi.Models;
using TodoApi.Services.Interface;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    public AuthenticationController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    /// <summary>
    /// Register a new user
    /// </summary>

    [HttpPost("Register")]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _userService.RegisterUserAsync(model);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        return BadRequest("Invalid properties");
    }
}