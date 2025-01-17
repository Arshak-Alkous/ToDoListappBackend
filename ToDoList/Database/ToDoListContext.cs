using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Database
{
    public class ToDoListContext:DbContext
    {
        public ToDoListContext(DbContextOptions<ToDoListContext> options):base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }    
    }
}
