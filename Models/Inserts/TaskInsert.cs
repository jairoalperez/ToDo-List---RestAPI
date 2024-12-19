using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static ToDoList_RestAPI.Models.Task;

namespace ToDoList_RestAPI.Models
{
    public class TaskInsert
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StimatedDate { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public ECurrentState CurrentState { get; set; }
        [Required]
        public EPriority Priority { get; set; }
    }
}