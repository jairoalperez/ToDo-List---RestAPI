using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList_RestAPI.Helpers
{
    public static class Messages
    {
        public static class Task
        {
            public const string NoTasks             = "Could not find any tasks";
            public const string NoUserTasks         = "Could not find any tasks associated to this user";
            public const string NotFound            = "This task was not found";
            public const string UserExistentTask    = "This user already created a task with the same name";
            public const string TaskCreated         = "Task created successfully";
            public const string TaskEdited          = "Task edited successfully";
            public const string AllTasksDeleted     = "All tasks were deleted successfully";
            public const string UserTasksDeleted    = "All tasks associated with this user were deleted successfully";
            public const string TaskDeleted         = "Task deleted successfully";
        }
    }
}