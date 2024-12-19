using Microsoft.EntityFrameworkCore;
using ToDoList_RestAPI.Models;
using Task = ToDoList_RestAPI.Models.Task;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public required DbSet<Task> Tasks { get; set; }
    public required DbSet<User> Users { get; set; }

}