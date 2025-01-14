using System.ComponentModel.DataAnnotations;

namespace ToDoList_RestAPI.Models
{
    public class ChangePasswordInsert
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}