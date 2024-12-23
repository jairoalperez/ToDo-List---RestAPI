using System.ComponentModel.DataAnnotations;
using static ToDoList_RestAPI.Models.Task;

namespace ToDoList_RestAPI.Models
{
    public class LoginInsert
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}