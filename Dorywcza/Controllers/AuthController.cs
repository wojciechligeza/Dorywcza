using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Dorywcza.Models.Auth;
using Dorywcza.Services.AuthService;
using Dorywcza.Services.AuthService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dorywcza.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthProvider _authProvider;
        private readonly AuthConfiguration _authConfiguration;

        public AuthController(IAuthProvider authProvider, IMapper mapper,
                              IOptions<AuthConfiguration> authConfiguration)
        {
            _mapper = mapper;
            _authProvider = authProvider;
            _authConfiguration = authConfiguration.Value;
        }

        // POST: Auth/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult AuthenticateUser([FromBody]UserAuth userAuth)
        {
            var user = _authProvider.AuthenticateUser(userAuth.Username, userAuth.Password);

            if (user == null) return BadRequest("Nazwa użytkownika lub hasło są nieprawidłowe");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(
                new
                {
                    Id = user.UserId,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = tokenString
                });
        }

        // POST: Auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody]UserRegister userRegister)
        {
            var user = _mapper.Map<User>(userRegister);

            try
            {
                _authProvider.RegisterUser(user, userRegister.Password);
                return Ok();
            }
            catch (AppException e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: Auth
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _authProvider.GetUsers();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        // GET: Auth/1
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _authProvider.GetUser(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        // PUT: Auth/1
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, [FromBody]UserUpdate userUpdate)
        {
            // Mapping model to entity and setting id
            var user = _mapper.Map<User>(userUpdate);
            user.UserId = id;

            try
            {
                _authProvider.PutUser(user, userUpdate.Password);
                return Ok("User updated");
            }
            catch (AppException e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: Auth/1
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _authProvider.DeleteUser(id);
            return Ok("User deleted");
        }
    }
}