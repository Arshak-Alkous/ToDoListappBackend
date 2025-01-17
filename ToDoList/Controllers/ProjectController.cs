using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Database;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ToDoListContext _context;
        public ProjectController(ToDoListContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects= await _context.Projects.ToListAsync();
            return Ok(projects);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewProject([FromBody]Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllProjects), new {id=project.Id},project);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> FindProject(int id)
        {
            var project= await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id , [FromBody]Project project)
        {
            var projectToUpdate=await _context.Projects.FindAsync(id);
            if (projectToUpdate==null) return NotFound();
            projectToUpdate.Id = project.Id;
            projectToUpdate.Name = project.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var projectToDelete = await _context.Projects.FindAsync(id);
            if (projectToDelete==null) return NotFound();
            _context.Projects.Remove(projectToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
