using Microsoft.EntityFrameworkCore;

namespace TodoListAPI.Models
{
    public class TodoItemContext : DbContext
    {
            public TodoItemContext(DbContextOptions<TodoItemContext> options) : base(options)
            {
            }

            public DbSet<TodoItem> TodoItems { get; set; }
            public DbSet<User> Users { get; set; }
    }
}
