using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(IConfiguration configuration, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IEmailService emailService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet("activate")]
        public async Task<IActionResult> ActivateAccount(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Redirect("https://miododstaniula.pl/");
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

            var result = await _signInManager.PasswordSignInAsync(userLoginData.Email!, userLoginData.Password!, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized("Podany email lub hasło są nieprawidłowe.");
            }

            if (string.IsNullOrEmpty(userLoginData.Email))
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(userLoginData.Email);
            if (user == null || !user.EmailConfirmed)
            {
                return Unauthorized("Konto nie zostało aktywowane.");
            }
            var roles = await _userManager.GetRolesAsync(user!);
            var token = GenerateJwtTokenForUser(userLoginData.Email);

            return Ok(new { Token = token, Roles = roles, user!.Name, UserId = user.Id });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register userRegisterData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(userRegisterData.Email);
            if (existingUser != null)
            {
                return BadRequest("PODANY ADRES EMAIL JEST JUŻ ZAREJESTROWANY.");
            }

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
                PhoneNumber = userRegisterData.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, userRegisterData.Password!);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Client");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                await _emailService.SendActivationEmail(newUser.Email, newUser.Id, newUser.Name, token);
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        private string GenerateJwtTokenForUser(string userName)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is not set in the configuration.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
