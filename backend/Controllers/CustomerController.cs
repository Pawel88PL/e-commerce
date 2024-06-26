using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
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

        [HttpPost("{userId}/change-password")]
        public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                var incorrectPasswordError = changePasswordResult.Errors.FirstOrDefault(e => e.Code == "PasswordMismatch");
                if (incorrectPasswordError != null)
                {
                    return BadRequest(new { Message = "Nieprawidłowe aktualne hasło." });
                }

                return BadRequest(new { Message = "Nie udało się zmienić hasła. Spróbuj ponownie." });
            }

            return Ok( new {Message = "Hasło zostało pomyślnie zmienione." });
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


        [HttpGet("get-all")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _customerService.GetAllUsersAsync();
                if (users == null || users.Count == 0)
                {
                    return NotFound("Nie znaleziono żadnych użytkowników.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas pobierania użytkowników.");
                return StatusCode(500, "Wystąpił błąd podczas pobierania użytkowników.");
            }
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