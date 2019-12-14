using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.DTO;
using ApiServer_Example.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer_Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var result = new List<UserDto>();
            var users = await _userRepository.GetAllUsers();
            foreach (var user in users)
            {
                result.Add(new UserDto
                {
                    Email = user.Email,
                    Modulus = user.RsaKeys.Modulus,
                    Exponent = user.RsaKeys.Exponent
                });
            }

            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null)
                return BadRequest(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Id",new[]{"Niepoprawne id!"}}
                    }
                });

            return Ok(new UserDto
            {
                Email = user.Email,
                Modulus = user.RsaKeys.Modulus,
                Exponent = user.RsaKeys.Exponent
            });
        }
    }
}