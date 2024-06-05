using BestChoice.API.Models;
using BestChoice.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace BestChoice.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminController(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = appDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            int totalMemberCount = await _userManager.Users.CountAsync();
            int productCount = await _context.Products.CountAsync(); // Remove this line if product management is handled in a separate controller
            int nonAdminMemberCount = _roleManager.Roles.Count(m => m.Name == "Üye");
            int adminMemberCount = _roleManager.Roles.Count(m => m.Name == "Admin");

            var result = new
            {
                TotalMemberCount = totalMemberCount,
                ProductCount = productCount, // Remove this line if product management is handled in a separate controller
                NonAdminMemberCount = nonAdminMemberCount,
                AdminMemberCount = adminMemberCount
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("CheckAdmin")]
        public async Task<IActionResult> CheckAdmin()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            return Ok(new { IsAdmin = isAdmin });
        }

        [HttpGet]
        [Route("Members")]
        public async Task<IActionResult> Members()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
            }).ToList();

            return Ok(userDtos);
        }

        [HttpGet]
        [Route("MembersSearch")]
        public async Task<IActionResult> MembersSearch(string searchTerm)
        {
            var users = await _userManager.Users.Where(u =>
                u.UserName.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                u.FullName.Contains(searchTerm)
            ).ToListAsync();

            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
            }).ToList();

            return Ok(userDtos);
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet]
        [Authorize]
        [Route("GetRole/{id}")]
        public async Task<IActionResult> GetRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound("Role not found."); // Return 404 if role doesn't exist
            }

            // Convert role object to a DTO if needed (optional)
            var roleDto = _mapper.Map<RoleDto>(role);

            return Ok(roleDto);
        }

        [HttpPost]
        [Route("RoleAdd")]
        public async Task<IActionResult> RoleAdd(AppRole model)
        {
            var existingRole = await _roleManager.FindByNameAsync(model.Name);
            if (existingRole != null)
            {
                return BadRequest(new { Message = "Bu isimde bir rol zaten mevcut." });
            }

            var newRole = new AppRole { Name = model.Name };
            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Rol başarıyla eklendi." });
            }

            return BadRequest(new { Errors = result.Errors });
        }

        [HttpPut]
        [Route("RoleEdit/{id}")]
        public async Task<IActionResult> RoleEdit(string id, AppRole model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { Message = "Geçersiz rol ID'si." });
            }

            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound(new { Message = "Bu ID'de bir rol bulunamadı." });
            }

            existingRole.Name = model.Name;
            var updateResult = await _roleManager.UpdateAsync(existingRole);

            if (updateResult.Succeeded)
            {
                return Ok(new { Message = "Rol başarıyla güncellendi.", status=true});
        }

            return BadRequest(new { Errors = updateResult.Errors });
        }

        [HttpDelete]
        [Route("RoleRemove/{id}")]
        public async Task<IActionResult> RoleRemove(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { Message = "Bu ID'de bir rol bulunamadı." });
            }

            // Check if the role is assigned to any users
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
            {
                return BadRequest(new { Message = "Bu rol şu anda kullanıcılar tarafından kullanılıyor ve silinemiyor." });
            }

            var deleteResult = await _roleManager.DeleteAsync(role);

            if (deleteResult.Succeeded)
            {
                return Ok(new { Message = "Rol başarıyla silindi." });
            }

            return BadRequest(new { Errors = deleteResult.Errors });
        }

        [HttpGet]
        [Route("UserEdit/{id}")]
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Bu ID'de bir kullanıcı bulunamadı." });
            }

            var userDto = new UserEditDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                CurrentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            };

            return Ok(userDto);
        }

        [HttpPut]
        [Route("UserEdit/{id}")]
        public async Task<IActionResult> UserEdit(string id, UserEditDto model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { Message = "Geçersiz kullanıcı ID'si." });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Bu ID'de bir kullanıcı bulunamadı." });
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.UserName;

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                // Update user roles if necessary
                if (model.CurrentRole != model.NewRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, model.CurrentRole);
                    await _userManager.AddToRoleAsync(user, model.NewRole);
                }

                return Ok(new { Message = "Kullanıcı bilgileri başarıyla güncellendi." });
            }

            return BadRequest(new { Errors = updateResult.Errors });
        }

        [HttpPut]
        [Route("UserUnblock/{id}")]
        public async Task<IActionResult> UserUnblock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Bu ID'de bir kullanıcı bulunamadı." });
            }

            var isCurrentlyBlocked = await _userManager.IsLockedOutAsync(user);
            if (!isCurrentlyBlocked)
            {
                return BadRequest(new { Message = "Kullanıcı zaten bloke edilmemiş durumda." });
            }

            await _userManager.SetLockoutEndDateAsync(user, null);
            return Ok(new { Message = "Kullanıcı bloke edilmesi başarıyla kaldırıldı." });
        }
    }
}