using System.Collections;
using Microsoft.AspNetCore.Mvc;
using ToDoList_RestAPI.Models;
using ToDoList_RestAPI.Services;
using ToDoList_RestAPI.Helpers;
using Task = ToDoList_RestAPI.Models.Task;
using TaskInsert = ToDoList_RestAPI.Models.TaskInsert;

namespace ToDoList_RestAPI.Controllers;

[ApiController]
[Route("[controller]/task")]
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
    public ActionResult<IEnumerable<Task>> GetUserTasks([FromRoute] int userId)
    {
        var userTasks = TasksDataStore.Current.Tasks.Where(i => i.UserId == userId).ToList();
        if (userTasks.Count < 1) 
        {
            return Problem(Messages.Task.NoUserTasks);
        }
        return Ok(userTasks);
    }


    [HttpGet("{taskId}")]
    public ActionResult<Task> GetTask([FromRoute] int taskId)
    {
        var task = TasksDataStore.Current.Tasks.FirstOrDefault(i => i.Id == taskId);
        if (task == null)
        {
            return Problem(Messages.Task.NotFound);
        }

        return Ok(task);
    }


    [HttpPost("create")]
    public ActionResult<Task> PostTask([FromBody] TaskInsert taskInsert)
    {

        var maxTaskId = TasksDataStore.Current.Tasks.DefaultIfEmpty(new Task {Id = 0}).Max(x => x.Id);

        var newTask = new Task()
        {
            Id = maxTaskId + 1,
            UserId = taskInsert.UserId,
            Title = taskInsert.Title,
            Description = taskInsert.Description,
            CreationDate = DateTime.Now,
            StimatedDate = taskInsert.StimatedDate,
            StartingDate = taskInsert.StartingDate,
            CompletionDate = taskInsert.CompletionDate,
            IsCompleted = taskInsert.IsCompleted,
            CurrentState = taskInsert.CurrentState,
            Priority = taskInsert.Priority
        };

        TasksDataStore.Current.Tasks.Add(newTask);

        return CreatedAtAction(
            nameof(GetTask),
            new {taskId = newTask.Id},
            new {
                Message = Messages.Task.TaskCreated,
                Task = newTask
            }
        );
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
            new {taskId = task.Id},
            new {
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