using System.Reflection.PortableExecutable;
using System.Security.Claims;
using empcoreapiproj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace empcoreapiproj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IConfiguration _config;
        public AuthenticateController(UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> rolemanager, IConfiguration config)
        {
            _usermanager = usermanager;
            _rolemanager = rolemanager;
            _config = config;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login log)
        {
            var user = await _usermanager.FindByNameAsync(log.Username);
            if (user != null && await _usermanager.CheckPasswordAsync(user, log.Passwprd))
            {
                var userRoles = await _usermanager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSignkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _config["JWT:ValidIssuer"],
                    audience: _config["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(15),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignkey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),


                });
            }
            return Unauthorized();

        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] Register mod)
        {
            var userExist = await _usermanager.FindByNameAsync(mod.Username);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Already Exists" });
            ApplicationUser user = new ApplicationUser()
            {
                Email = mod.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = mod.Username
            };
            var result = await _usermanager.CreateAsync(user, mod.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed" });
            }
            return Ok(new Response { Status = "Success", Message = "User created succesfully" });
        }
        [HttpPost]
        [Route("Role-admin")]
        public async Task<IActionResult> AdminRole([FromBody] Register mod)
        {
            var userExixst = await _usermanager.FindByNameAsync(mod.Username);
            if (userExixst != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed" });
            ApplicationUser user = new ApplicationUser()
            {
                Email = mod.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = mod.Username

            };
            var result = await _usermanager.CreateAsync(user, mod.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed" });
            }
            if (!await _rolemanager.RoleExistsAsync(UserRoles.Admin))
                await _rolemanager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _rolemanager.RoleExistsAsync(UserRoles.User))
                await _rolemanager.CreateAsync(new IdentityRole(UserRoles.User));
            if (!await _rolemanager.RoleExistsAsync(UserRoles.Admin))
            {
                await _usermanager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _rolemanager.RoleExistsAsync(UserRoles.Admin))
            {
                await _usermanager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully" });
        }
    }
}