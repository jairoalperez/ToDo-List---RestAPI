using System.Collections;
using Microsoft.AspNetCore.Mvc;
using ToDoList_RestAPI.Models;
using ToDoList_RestAPI.Services;
using ToDoList_RestAPI.Helpers;
using Task = ToDoList_RestAPI.Models.Task;

namespace ToDoList_RestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoListController : ControllerBase
{
    [HttpGet("all")]
    public ActionResult<IEnumerable<Task>> GetAllTasks()
    {
        var allTasks = TasksDataStore.Current.Tasks;

        if (allTasks.Count < 1)
        {
            return Problem(Messages.Task.NoTasks);
        }

        return Ok(allTasks);
    }

    [HttpGet("user/{userId}")]
    public ActionResult<IEnumerable<Task>> GetUserTasks([FromRoute] int UserId)
    {
        return Ok();
    }

    [HttpGet("{taskId}")]
    public ActionResult<Task> GetTask([FromRoute] int TaskId)
    {
        return Ok();
    }

    [HttpPost("create")]
    public ActionResult<Task> PostTask()
    {
        return Ok();
    }

    [HttpPut("edit/{taskId}")]
    public ActionResult<Task> PutTask([FromRoute] int taskId)
    {
        return Ok();
    }

    [HttpDelete("delete/all")]
    public ActionResult<IEnumerable<Task>> DeleteAllTasks()
    {
        return Ok();
    }

    [HttpDelete("delete/user/{userId}")]
    public ActionResult<IEnumerable<Task>> DeleteUserTasks()
    {
        return Ok();
    }

    [HttpDelete("delete/{task}")]
    public ActionResult<Task> DeleteTask()
    {
        return Ok();
    }
}