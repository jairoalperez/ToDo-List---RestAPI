using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList_RestAPI.Models
{
    public class Task
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationDate { get; set; }
        public DateTime? StimatedDate { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool IsCompleted { get; set; }
        public ECurrentState CurrentState { get; set; }
        public EPriority Priority { get; set; }

        public enum ECurrentState
        {
            NotStarted,
            InProgress,
            Done,
            Abandoned
        }

        public enum EPriority
        {
            low,
            medium,
            high
        }
    }
}