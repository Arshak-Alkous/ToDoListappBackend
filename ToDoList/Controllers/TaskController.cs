using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Database;
using ToDoList.Models;
namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ToDoListContext _context;
        public TaskController(ToDoListContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewTask([FromBody] Models.Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllTasks), new { id = task.TaskId }, task);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> FindTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Models.Task task)
        {
            var taskToUpdate = await _context.Tasks.FindAsync(id);
            if (taskToUpdate == null) return NotFound();
            taskToUpdate.Name = task.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var taskToDelete = await _context.Tasks.FindAsync(id);
            if (taskToDelete == null) return NotFound();
            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
