using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public AccountController(
            IAccountService accountService,
            ICustomerService customerService,
            IEmailService emailService,
            ITokenService tokenService)
        {
            _accountService = accountService;
            _customerService = customerService;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        [HttpGet("activate")]
        public async Task<IActionResult> ActivateAccount(string userId, string token)
        {
            var user = await _accountService.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            var result = await _accountService.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Redirect("https://miododstaniula.pl/cart");
            }
            else
            {
                return BadRequest("Nie udało się aktywować konta.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login userLoginData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.PasswordSignInAsync(userLoginData.Email!, userLoginData.Password!);

            if (!result)
            {
                return Unauthorized("Podany email lub hasło są nieprawidłowe.");
            }

            var user = await _accountService.FindByEmailAsync(userLoginData.Email);
            if (user == null || !user.EmailConfirmed)
            {
                return Unauthorized("Konto nie zostało aktywowane.");
            }

            var token = _accountService.GenerateJwtTokenForUser(user);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register userRegisterData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _accountService.FindByEmailAsync(userRegisterData.Email);

            if (existingUser != null && existingUser.IsGuestClient == false && userRegisterData.IsGuestClient == true)
            {
                return BadRequest("Podany adres email jest już zarejestrowany. Zaloguj się do swojego konta, aby złożyć zamówienie.");
            }

            if (existingUser != null && userRegisterData.IsGuestClient == true)
            {
                var updateGuestUserAsync = await _customerService.UpdateGuestUserAsync(existingUser.Id, userRegisterData);
                if (updateGuestUserAsync == true)
                {
                    return Ok(new { UserId = existingUser.Id });
                }
                else
                {
                    return BadRequest("Nie udało się zaktualizować danych klienta składającego zamówienie jako gość.");
                }
            }

            if (existingUser != null && existingUser.IsGuestClient == true)
            {
                var registerGuestUser = await _customerService.UpdateGuestUserAsync(existingUser.Id, userRegisterData);
                if (registerGuestUser == true)
                {
                    await _accountService.AddToRoleAsync(existingUser, "Client");
                    if (existingUser.Email != null)
                    {
                        var token = await _accountService.GenerateEmailConfirmationTokenAsync(existingUser);
                        await _emailService.SendActivationEmail(existingUser.Email, existingUser.Id, existingUser.Name, token);
                        return Ok(new { UserId = existingUser.Id });
                    }
                }
                else
                {
                    return BadRequest("Nie udało się zmienić konta klienta.");
                }
            }

            if (existingUser != null && userRegisterData.IsGuestClient == false)
            {
                return BadRequest("Podany adres email jest już zarejestrowany.");
            }
            else
            {
                var newUser = new UserModel
                {
                    UserName = userRegisterData.Email,
                    Email = userRegisterData.Email,
                    Name = userRegisterData.Name,
                    Surname = userRegisterData.Surname,
                    City = userRegisterData.City,
                    PostalCode = userRegisterData.PostalCode,
                    Street = userRegisterData.Street,
                    Address = userRegisterData.Address,
                    PhoneNumber = userRegisterData.PhoneNumber,
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                    IsGuestClient = userRegisterData.IsGuestClient,
                    TermsAccepted = userRegisterData.TermsAccepted
                };

                var result = await _accountService.CreateAsync(newUser, userRegisterData.Password!);

                if (result.Succeeded)
                {
                    await _accountService.AddToRoleAsync(newUser, "Client");
                    if (newUser.IsGuestClient == false)
                    {
                        var token = await _accountService.GenerateEmailConfirmationTokenAsync(newUser);
                        await _emailService.SendActivationEmail(newUser.Email, newUser.Id, newUser.Name, token);
                    }
                    return Ok(new { UserId = newUser.Id });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _tokenService.RevokeTokenAsync(token);
            await _accountService.SignOutAsync();
            return Ok();
        }
    }
}
