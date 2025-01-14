using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ToDoList_RestAPI.Helpers;

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

    
}