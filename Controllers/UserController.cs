using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ToDoList_RestAPI.Helpers;
using ToDoList_RestAPI.Models;

namespace ToDoList_RestAPI.Controllers;

[ApiController]
[Route("api/users")]

public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
    public async Task<ActionResult> GetAll([FromQuery] int page)
    {
        try
        {
            if (page <= 0)
            {
                return BadRequest(Messages.API.PageZero);
            }

            var usersQuery = _context.Users
                .OrderBy(u => u.UserId)
                .Skip((page - 1) * 50)
                .Take(50);

            var allUsers = await usersQuery
            .Select(u => new
            {
                u.UserId,
                u.Username,
                u.BirthDate
            }).ToListAsync();

            var totalItems = await _context.Users.CountAsync();
            var totalPages = (totalItems / 50) + 1;

            if (allUsers.Count == 0)
            {
                return NotFound(new
                {
                    Message = Messages.User.NotFound,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    CurrentPage = page
                });
            }

            return Ok(
                new
                {
                    CurrentPage = page,
                    PageSize = 50,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Users = allUsers
                });
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }

    [HttpGet("searchby/id/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var user = await _context.Users.Include(u => u.Tasks).FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound(Messages.User.NotFound);
            }

            return Ok(new
            {
                user.UserId,
                user.Username,
                user.FirstName,
                user.LastName,
                user.BirthDate,
                user.Email,
                user.PhoneNumber
            });
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }

    [HttpGet("searchby/username/{username}")]
    public async Task<IActionResult> GetByUsername([FromRoute] string username)
    {
        try
        {
            var user = await _context.Users.Include(u => u.Tasks).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound(Messages.User.NotFound);
            }

            return Ok(new
            {
                user.UserId,
                user.Username,
                user.FirstName,
                user.LastName,
                user.BirthDate,
                user.Email,
                user.PhoneNumber
            });
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }

    [HttpGet("searchby/email/{email}")]
    public async Task<IActionResult> GetByEmail([FromRoute] string email)
    {
        try
        {
            var user = await _context.Users.Include(u => u.Tasks).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound(Messages.User.NotFound);
            }

            return Ok(new
            {
                user.UserId,
                user.Username,
                user.FirstName,
                user.LastName,
                user.BirthDate,
                user.Email,
                user.PhoneNumber
            });
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] UserInsert userInsert)
    {
        try
        {
            var userToEdit = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (userToEdit == null)
            {
                return NotFound(Messages.User.NotFound);
            }

            if (_context.Users.Any(u => (u.Username == userInsert.Username && u.UserId != id) || (u.Email == userInsert.Email && u.UserId != id)))
            {
                return BadRequest(Messages.User.UserExists);
            }

            userToEdit.Username = userInsert.Username;
            userToEdit.FirstName = userInsert.FirstName;
            userToEdit.LastName = userInsert.LastName;
            userToEdit.BirthDate = userInsert.BirthDate;
            userToEdit.Email = userInsert.Email;
            userToEdit.PhoneNumber = userInsert.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok(Messages.User.Edited);
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (userToDelete == null)
            {
                return NotFound(Messages.User.NotFound);
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return Ok(Messages.User.Deleted);
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }
}