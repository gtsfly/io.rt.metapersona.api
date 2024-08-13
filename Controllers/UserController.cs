using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.Models;
using otel_advisor_webApp.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace otel_advisor_webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HotelContext _context;

        public UserController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetDef_User()
        {
            var def_user = await _context.Def_User
                .Select(u => new UserDto
                {
                    user_id = u.user_id,
                    name = u.name,
                    email = u.email
                }).ToListAsync();
            return Ok(def_user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Def_User
                .Where(u => u.user_id == id)
                .Select(u => new UserDto
                {
                    user_id = u.user_id,
                    name = u.name,
                    email = u.email
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("getUserByName/{name}")]
        public async Task<ActionResult<int>> GetUserIdByName(string name)
        {
            var user = await _context.Def_User
                .Where(u => u.name == name)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user.user_id);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser([FromBody] UserDto userDto)
        {
            // Check current user
            var existingUser = await _context.Def_User
                .FirstOrDefaultAsync(u => u.email == userDto.email);

            if (existingUser != null)
            {
                // If user is already exist, return it
                return Conflict(new UserDto
                {
                    user_id = existingUser.user_id,
                    name = existingUser.name,
                    email = existingUser.email
                });
            }

            // Create new user
            var user = new User
            {
                name = userDto.name,
                email = userDto.email
            };

            _context.Def_User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.user_id }, new UserDto
            {
                user_id = user.user_id,
                name = user.name,
                email = user.email
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] UserDto userDto)
        {
            var user = await _context.Def_User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.name = userDto.name;
            user.email = userDto.email;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Def_User.Any(e => e.user_id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Def_User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Def_User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
