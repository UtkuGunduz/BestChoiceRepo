using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BestChoice.API.Dtos;
using BestChoice.API.Models;

namespace BestChoice.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ResultDto result = new ResultDto();
        public UserController(UserManager<AppUser> userManager, IMapper mapper, RoleManager<AppRole> roleManager, IConfiguration configuration, SignInManager<AppUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetUserList")]
        [Authorize(Roles = "Admin")]
        public List<UserDto> GetUserList()
        {
            var users = _userManager.Users.ToList();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;
        }

        [HttpGet("GetById/{id}")]
        public UserDto GetById(string id)
        {
            var user = _userManager.Users.Where(s => s.Id == id).SingleOrDefault();
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        [Authorize]
        [HttpGet("GetUserProfile")]
        public async Task<ActionResult<UserDto>> GetUserProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new ResultDto { Status = false, Message = "Kullanıcı Bulunamadı!" });
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }


        [HttpPost("Add")]
        public async Task<ResultDto> Add(RegisterDto dto)
        {
            var identityResult = await _userManager.CreateAsync(new() { UserName = dto.UserName, Email = dto.Email, FullName = dto.FullName, PicUrl = "defaultProfilePhoto.jpg" }, dto.Password);

            if (!identityResult.Succeeded)
            {
                result.Status = false;
                foreach (var item in identityResult.Errors)
                {
                    result.Message += "<p>" + item.Description + "<p>";
                }

                return result;
            }
            var user = await _userManager.FindByNameAsync(dto.UserName);
            var roleExist = await _roleManager.RoleExistsAsync("Uye");
            if (!roleExist)
            {
                var role = new AppRole { Name = "Uye" };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, "Uye");
            result.Status = true;
            result.Message = "Üye Eklendi";
            return result;
        }

        [HttpPost("SignIn")]
        public async Task<ResultDto> SignIn(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user is null)
            {
                result.Status = false;
                result.Message = "Üye Bulunamadı!";
                return result;
            }
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordCorrect)
            {
                result.Status = false;
                result.Message = "Kullanıcı Adı veya Parola Geçersiz!";
                return result;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("userPicUrl", user.PicUrl),

            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateJWT(authClaims);

            result.Status = true;
            result.Message = token;
            return result;

        }
        private string GenerateJWT(List<Claim> claims)
        {

            var accessTokenExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["AccessTokenExpiration"]));


            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: accessTokenExpiration,
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }

        [HttpPut("UpdateUser")]
        public async Task<ResultDto> UpdateUser(UpdateUserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
            {
                result.Status = false;
                result.Message = "Kullanıcı Bulunamadı!";
                return result;
            }

            user.Email = dto.Email;
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;

            var identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                result.Status = false;
                result.Message = "Kullanıcı güncelleme işlemi başarısız!";
                return result;
            }

            result.Status = true;
            result.Message = "Kullanıcı bilgileri güncellendi";
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<ResultDto> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                result.Status = false;
                result.Message = "Kullanıcı Bulunamadı!";
                return result;
            }

            var identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
            {
                result.Status = false;
                result.Message = "Kullanıcı silme işlemi başarısız!";
                return result;
            }

            result.Status = true;
            result.Message = "Kullanıcı silindi";
            return result;
        }

        [Authorize]
        [HttpPost("ChangePassword/{id}")]
        public async Task<ResultDto> ChangePassword(string id, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                result.Status = false;
                result.Message = "Kullanıcı Bulunamadı!";
                return result;
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                result.Status = false;
                result.Message = "Şifre değiştirme işlemi başarısız!";
                return result;
            }

            result.Status = true;
            result.Message = "Şifre değiştirildi";
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleAdd addRole)
        {
            var user = await _userManager.FindByIdAsync(addRole.UserId);
            if (user == null)
            {
                return NotFound($"Kullanıcı bulunamadı.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(addRole.RoleName);
            if (!roleExists)
            {
                return BadRequest($"Rol mevcut değil.");
            }

            var result = await _userManager.AddToRoleAsync(user, addRole.RoleName);
            if (result.Succeeded)
            {
                return Ok($"Rol {user.UserName} adlı kullanıcıya atandı.");
            }
            else
            {
                return BadRequest($"Rol {user.UserName} adlı kullanıcıya atanamadı. Errors: {string.Join(", ", result.Errors)}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetRoleList")]
        public async Task<IActionResult> GetRoleList()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            if (roles == null || roles.Count == 0)
            {
                return NotFound("Roller bulunamadı.");
            }

            var roleDtos = roles.Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();

            return Ok(roleDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsersByRole/{roleName}")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return NotFound($"Rol '{roleName}' bulunamadı.");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            if (usersInRole == null || usersInRole.Count == 0)
            {
                return NotFound($"'{roleName}' rolüne sahip kullanıcı bulunamadı.");
            }

            var userNames = usersInRole.Select(user => user.UserName).ToList();

            return Ok(userNames);
        }

        [Authorize]
        [HttpPost("Upload")]
        public async Task<ResultDto> Upload(UploadDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                result.Status = false;
                result.Message = "Kayıt Bulunmadı!";
                return result;
            }

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/Profile");
            string userPic = user.PicUrl;

            if (userPic != "defaultProfilePhoto.jpg")
            {

                var userPicUrl = Path.Combine(path, userPic);

                if (System.IO.File.Exists(userPicUrl))
                {
                    System.IO.File.Delete(userPicUrl);
                }
            }
            string data = dto.PicData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imageBytes = Convert.FromBase64String(base64);
            string filePath = Guid.NewGuid().ToString() + dto.PicExt;


            var picPath = Path.Combine(path, filePath);

            System.IO.File.WriteAllBytes(picPath, imageBytes);

            user.PicUrl = filePath;
            await _userManager.UpdateAsync(user);

            result.Status = true;
            result.Message = "Profil Fotoğrafı Güncellendi";

            return result;
        }
    }
}
