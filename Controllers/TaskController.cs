using Microsoft.AspNetCore.Mvc;
using ToDoList_RestAPI.Services;
using ToDoList_RestAPI.Helpers;
using Task = ToDoList_RestAPI.Models.Task;
using TaskInsert = ToDoList_RestAPI.Models.TaskInsert;
using Microsoft.EntityFrameworkCore;

namespace ToDoList_RestAPI.Controllers;

[ApiController]
[Route("api/task")]
public class ToDoListController : ControllerBase
{
    private readonly AppDbContext _context;
    public ToDoListController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTasks()
    {
        try
        {
            var allTasks = await _context.Tasks.ToListAsync();

            if (allTasks.Count < 1)
            {
                return NotFound(Messages.Task.NoTasks);
            }

            return Ok(allTasks);
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<Task>> GetUserTasks([FromRoute] int userId)
    {
        try
        {
            var userTasks = await _context.Tasks.Where(i => i.UserId == userId).ToListAsync();
            if (userTasks.Count < 1)
            {
                return Problem(Messages.Task.NoUserTasks);
            }
            return Ok(userTasks);
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }

    [HttpGet("{taskId}")]
    public async Task<ActionResult> GetTask([FromRoute] int taskId)
    {
        try
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(i => i.Id == taskId);
            if (task == null)
            {
                return Problem(Messages.Task.NotFound);
            }

            return Ok(task);
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<Task>> PostTask([FromBody] TaskInsert taskInsert)
    {
        try
        {
            var newTask = new Task()
            {
                UserId = taskInsert.UserId,
                Title = taskInsert.Title,
                Description = taskInsert.Description,
                StimatedDate = taskInsert.StimatedDate,
                StartingDate = taskInsert.StartingDate,
                CompletionDate = taskInsert.CompletionDate,
                IsCompleted = taskInsert.IsCompleted,
                CurrentState = taskInsert.CurrentState,
                Priority = taskInsert.Priority
            };

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = Messages.Task.TaskCreated,
                Task = newTask
            }
            );
        }
        catch (Exception ex)
        {
            return Problem(Messages.Database.ProblemRelated, ex.Message);
        }
    }


    [HttpPut("edit/{taskId}")]
    public ActionResult<Task> PutTask([FromRoute] int taskId, [FromBody] TaskInsert taskInsert)
    {
        var task = TasksDataStore.Current.Tasks.FirstOrDefault(i => i.Id == taskId);
        if (task == null)
        {
            return Problem(Messages.Task.NotFound);
        }

        task.Title = taskInsert.Title;
        task.Description = taskInsert.Description;
        task.StimatedDate = taskInsert.StimatedDate;
        task.StartingDate = taskInsert.StartingDate;
        task.CompletionDate = taskInsert.CompletionDate;
        task.IsCompleted = taskInsert.IsCompleted;
        task.CurrentState = taskInsert.CurrentState;
        task.Priority = taskInsert.Priority;

        return CreatedAtAction(
            nameof(GetTask),
            new { taskId = task.Id },
            new
            {
                Message = Messages.Task.TaskEdited,
                Task = task
            }
        );
    }


    [HttpDelete("delete/all")]
    public ActionResult<IEnumerable<Task>> DeleteAllTasks()
    {
        var allTasks = TasksDataStore.Current.Tasks;

        if (allTasks.Count < 1)
        {
            return Problem(Messages.Task.NoTasks);
        }

        TasksDataStore.Current.Tasks.Clear();

        return Ok(Messages.Task.AllTasksDeleted);
    }


    [HttpDelete("delete/user/{userId}")]
    public ActionResult<IEnumerable<Task>> DeleteUserTasks([FromRoute] int userId)
    {
        var userTasks = TasksDataStore.Current.Tasks.Where(i => i.UserId == userId).ToList();
        if (userTasks.Count < 1)
        {
            return Problem(Messages.Task.NoUserTasks);
        }

        TasksDataStore.Current.Tasks.RemoveAll(task => task.UserId == userId);

        return Ok(Messages.Task.UserTasksDeleted);
    }


    [HttpDelete("delete/{taskId}")]
    public ActionResult<Task> DeleteTask([FromRoute] int taskId)
    {
        var task = TasksDataStore.Current.Tasks.FirstOrDefault(x => x.Id == taskId);
        if (task == null)
        {
            return Problem(Messages.Task.NotFound);
        }

        TasksDataStore.Current.Tasks.Remove(task);

        return Ok(Messages.Task.TaskDeleted);
    }
}