using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList_RestAPI.Models;
using Task = ToDoList_RestAPI.Models.Task;

namespace ToDoList_RestAPI.Services
{
    public class TasksDataStore
    {
        public List<Task> Tasks { get; set; }

        public static TasksDataStore Current {get; } = new TasksDataStore();

        public TasksDataStore()
        {
            Tasks = new List<Task>()
            {
                new Task()
                {
                    Id = 1,
                    UserId = 1,
                    Title = "Learn C#",
                    Description = "I have to learn this as this will open to much more opportunities on development as it is one of the most used languages as for backend dev, software dev and game dev",
                    CreationDate = DateTime.Now,
                    StimatedDate = new DateTime(2024, 12, 31),
                    StartingDate = new DateTime(2024, 11, 11),
                    IsCompleted = false,
                    CurrentState = Models.Task.ECurrentState.InProgress,
                    Priority = Models.Task.EPriority.medium
                },
                new Task()
                {
                    Id = 2,
                    UserId = 1,
                    Title = "Learn ASP.Net Core",
                    Description = "ASP.Net Core is the C# Framework most used for backend development, this will open me a lot of opportunities",
                    CreationDate = DateTime.Now,
                    StimatedDate = new DateTime(2024, 01, 31),
                    StartingDate = new DateTime(2024, 11, 18),
                    IsCompleted = false,
                    CurrentState = Models.Task.ECurrentState.InProgress,
                    Priority = Models.Task.EPriority.high
                },
                new Task()
                {
                    Id = 3,
                    UserId = 1,
                    Title = "Learn Azure",
                    Description = "Azure is the microsoft cloud service platform. This will significantly increase my value",
                    CreationDate = DateTime.Now,
                    StimatedDate = new DateTime(2024, 02, 28),
                    StartingDate = new DateTime(2024, 11, 09),
                    IsCompleted = false,
                    CurrentState = Models.Task.ECurrentState.InProgress,
                    Priority = Models.Task.EPriority.high
                },
                new Task()
                {
                    Id = 4,
                    UserId = 1,
                    Title = "Learn SQL Server",
                    Description = "SQL is the database administration platform made by microsoft, this will complete my microsoft tools skillsbase",
                    CreationDate = DateTime.Now,
                    StimatedDate = new DateTime(2024, 03, 31),
                    IsCompleted = false,
                    CurrentState = Models.Task.ECurrentState.NotStarted,
                    Priority = Models.Task.EPriority.medium
                },
                new Task()
                {
                    Id = 5,
                    UserId = 1,
                    Title = "Learn React",
                    Description = "React is the most used front end development framework used by the moment",
                    CreationDate = DateTime.Now,
                    StimatedDate = new DateTime(2023, 06, 30),
                    StartingDate = new DateTime(2022, 08, 20),
                    CompletionDate = new DateTime(2023, 03, 19),
                    IsCompleted = true,
                    CurrentState = Models.Task.ECurrentState.Done,
                    Priority = Models.Task.EPriority.high
                },
                new Task()
                {
                    Id = 6,
                    UserId = 2,
                    Title = "Learn HTML",
                    Description = "HTML is the basic for web apps frontend structure",
                    CreationDate = DateTime.Now,
                    StimatedDate = new DateTime(2025, 12, 31),
                    IsCompleted = false,
                    CurrentState = Models.Task.ECurrentState.NotStarted,
                    Priority = Models.Task.EPriority.low
                },
                new Task()
                {
                    Id = 7,
                    UserId = 2,
                    Title = "Learn Javascript",
                    Description = "JS is the basic language for web apps programming",
                    CreationDate = DateTime.Now,
                    StimatedDate = new DateTime(2025, 12, 31),
                    IsCompleted = false,
                    CurrentState = Models.Task.ECurrentState.NotStarted,
                    Priority = Models.Task.EPriority.medium
                },
            };
        }
    }
}