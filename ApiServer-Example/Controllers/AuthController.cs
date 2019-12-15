using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using ApiServer_Example.Domains.DTO;
using ApiServer_Example.Domains.Models;
using ApiServer_Example.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer_Example.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService,IUserRepository userRepository,IMapper mapper)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthData>> Login([FromBody]LoginDto model)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Email",new[]{"Niepoprawny adres email lub hasło!"}}
                    }
                });
            }

            var passwordValid = _authService.VerifyPassword(model.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return BadRequest(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Password",new []{"Niepoprawny adres email lub hasło!"}}
                    }
                });
            }

            return Ok(await _authService.GetAuthData(user.Id));
        }

        [HttpPost("register")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var emailUniq = await _userRepository.IsEmailUniqAsync(model.Email);
            if (!emailUniq)
                return BadRequest(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        {"Email", new []{"Użytkownik o takim emailu już istnieje!"}}
                    }
                });

            var user = _mapper.Map<User>(model);
            user.PasswordHash = _authService.HashPassword(model.Password);
            using (var rsa = new RSACryptoServiceProvider())
            {
                user.RsaKeys = _mapper.Map<RSAKeys>(rsa.ExportParameters(true));
            }

            var userCreated = await _userRepository.CreateUserAsync(user);

            if (userCreated == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "ServerError",new []{"Tworzenie konta nie powiodło się!"}}
                    }
                });

            var userPath =  Path.Combine(Directory.GetCurrentDirectory(), userCreated.Id.ToString());
            Directory.CreateDirectory(userPath);

            return NoContent();
        }
    }
}