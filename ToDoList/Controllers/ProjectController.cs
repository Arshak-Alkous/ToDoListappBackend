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
            var projects= await _context.Projects
                .Include(p=>p.Tags)
                .Include(p=> p.Tasks)
                .ThenInclude(pt=>pt.Tags)
                .Select(p => new
                {
                    ProjectId = p.ProjectId,
                    Name = p.Name,
                    IsActive = p.IsActive,
                    Tags = p.Tags.Select(tag => new
                    {
                        TagId = tag.TagId,
                        Name = tag.Name
                    }).ToList(),
                    Tasks = p.Tasks.Select(task => new
                    {
                        TaskId = task.TaskId,
                        Name = task.Name,
                        Deadline = task.Deadline,
                        Tags = task.Tags.Select(tag => new
                        {
                            TagId = tag.TagId,
                            Name = tag.Name
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();
            return Ok(projects);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewProject([FromBody]ProjectTemp projectTemp)
        {
            var project = new Project
            {
                Name = projectTemp.Name,
                IsActive = projectTemp.IsActive
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllProjects), new {id=project.ProjectId},project);
        }
        [HttpPost("AddTaskToProject/{projectId}")]
        public async Task<IActionResult> AddTaskToProject(int projectId, [FromBody]TaskTemp taskTemp)
        {
            var project = _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if (project == null) 
            {
                return NotFound($"project with id {projectId} not found");
            }
            var task = new Models.Task
            {
                Name = taskTemp.Name,
                Deadline = taskTemp.Deadline,
                ProjectId = projectId,
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return Ok(task);
        }
        [HttpPost("AddTagToProject/{projectId}")]
        public async Task<IActionResult> AddTagToProject(int projectId, [FromBody] string tagName)
        {
            var project =await _context.Projects
                .Include(p=> p.Tags)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if (project == null)
            {
                return NotFound($"project with id {projectId} not found");
            }
            var tag = await _context.Tags.FirstOrDefaultAsync(t=> t.Name == tagName);
            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }
            project.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return Ok(tag);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> FindProject(int id)
        {
            var project= await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id , [FromBody] ProjectTemp projectTemp)
        {
            var projectToUpdate=await _context.Projects.FindAsync(id);
            if (projectToUpdate==null) return NotFound();
            projectToUpdate.Name = projectTemp.Name;
            projectToUpdate.IsActive = projectTemp.IsActive;
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
