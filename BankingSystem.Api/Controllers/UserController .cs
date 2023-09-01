using BankingSystem.Core.Interfaces;
using BankingSystem.Models.Entities;
using BankingSystem.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> CreateUser([Required] string name)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newUser = await _userService.CreateUser(name);
        return Ok(newUser);
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<User?>> GetUser(int userId)
    {
        var result = await _userService.GetUser(userId);
        if (result != null )
            return Ok(result);

        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<List<User>>> GetAllUser()
    {
        var result = await _userService.GetAllUser();
        if (result != null)
            return Ok(result);

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> UpdateUser([FromBody] UserModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.UpdateUser(request.UserId, request.Name!);
        if (result)
            return Ok(true);

        return NotFound(false);
    }

    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteUser(int userId)
    {
        var result = await _userService.DeleteUser(userId);
        if (result)
            return Ok(true);

        return NotFound(false);
    }

    [HttpPost("deposit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> Deposit([FromBody] UserAccountModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.Deposit(request.AccountId, request.Amount);
        if (result)
            return Ok(true);

        return NotFound(false);
    }


    [HttpPost("withdraw")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> Withdraw([FromBody] UserAccountModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.Withdraw(request.AccountId, request.Amount);
        if (result)
            return Ok(true);
        return NotFound(false);
    }
}