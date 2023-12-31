using Repository;
using Core.Domain.Entities;
using Core.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace api_yves.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly AppDbContext _context;

  public UsersController(AppDbContext context)
  {
    _context = context;
  }

  // GET: api/users
  [HttpGet]
  public async Task<ActionResult<IEnumerable<User>>> GetUsers()
  {
    var users = await _context.Users.AsNoTracking().ToListAsync();
    return Ok(users);
  }

  // GET: api/users/5
  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetUser(int id)
  {
    var user = await _context.Users.FindAsync(id);

    // if (user == null)
    // {
    //   return NotFound();
    // }

    return user == null ? NotFound() : Ok(user);
  }

  // POST: api/users
  [HttpPost]
  public async Task<ActionResult<CreateUserDTO>> PostUser(CreateUserDTO model)
  { 
    if(!ModelState.IsValid){
      return BadRequest();
    };

    var user = new User {
      Name = model.Name,
      BirthDate = model.BirthDate,
      CPF = model.CPF
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    var newUser = CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);

    return Ok(newUser);
  }

  // PUT: api/users/5
  [HttpPut("{id}")]
  public async Task<IActionResult> PutUser(int id, User user)
  {
    if (id != user.Id)
    {
      return BadRequest();
    }

    _context.Entry(user).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!UserExists(id))
      {
        return NotFound();
      }
      else
      {
        throw;
      }
    }

    return Ok("success");
  }

  // DELETE: api/users/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteUser(int id)
  {
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
      return NotFound();
    }

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool UserExists(int id)
  {
    return _context.Users.Any(e => e.Id == id);
  }

  // dummy method to test the connection
  [HttpGet("hello")]
  public string Test()
  {
    return "Hello World!";
  }
}