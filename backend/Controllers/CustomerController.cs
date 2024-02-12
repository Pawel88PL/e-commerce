using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Migrations;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;
        private readonly UserManager<UserModel> _userManager;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, UserManager<UserModel> userManager)
        {
            _customerService = customerService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUser(string userId)
        {
            var user = await _customerService.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            return Ok(user);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToUpdate = await _userManager.FindByIdAsync(userId);
            if (userToUpdate == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            if (userToUpdate.Email != userDto.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(userDto.Email);
                if (existingUser != null && existingUser.Id != userId)
                {
                    return BadRequest("Podany adres email jest już zarejestrowany.");
                }
            }

            try
            {
                var updateResult = await _customerService.UpdateUserAsync(userId, userDto);
                if (!updateResult)
                {
                    return NotFound($"User with ID {userId} not found.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {userId}.");
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }
    }
}