using BetsApi.Services;
using BetsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetsApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser(RegisterRequest request)
    {
        try
        {
            var createdUser = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(RegisterUser), createdUser);

        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> LoginUser(LoginRequest loginRequest)
    {
        try
        {
            var user = await _userService.LoginUser(loginRequest);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {

        var users = await _userService.GetAllUsersAsync();
        if (users != null)
        {
            return Ok(users);
        }
        return Ok(new List<User>());

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user != null)
        {
            return Ok(user);
        }
        return NotFound("User not found");
    }

    [HttpGet("username")]
    public async Task<ActionResult<User>> GetUserByUsername(string username)
    {
        try
        {
            var user = await _userService.GetUserByUserNameAsync(username);
            return Ok(user);
        }
        catch(InvalidDataException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> PutUser(User user, long id)
    {
        if(id != user.Id)
        {
            return BadRequest();
        }
        try
        {
            await _userService.UpdateUserAsync(id, user);
            return Ok();

        }
        catch(InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = "User not found." });
        }
        
    }




}
