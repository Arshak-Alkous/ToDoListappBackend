using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Database;
using ToDoList.Models;
namespace ToDoList.Controllers
{
     [ApiController]
     [Route("[controller]")]
     public class TagController : ControllerBase
     {
         private readonly ToDoListContext _context;
         public TagController(ToDoListContext context)
         {
             _context = context;
         }
         [HttpGet]
         public async Task<IActionResult> GetAllTags()
         {
             var tags= await _context.Tags.ToListAsync();
             return Ok(tags);
         }
         [HttpPost]
         public async Task<IActionResult> CreateNewTag([FromBody]Tag tag)
         {
             _context.Tags.Add(tag);
             await _context.SaveChangesAsync();
             return CreatedAtAction(nameof(GetAllTags), new {id=tag.Id},tag);
         }
         [HttpGet("{id}")]
         public async Task<IActionResult> FindTag(int id)
         {
             var tag= await _context.Tags.FindAsync(id);
             if (tag == null) return NotFound();
             return Ok(tag);
         }
         [HttpPut("{id}")]
         public async Task<IActionResult> UpdateTag(int id , [FromBody]Tag tag)
         {
             var tagToUpdate=await _context.Tags.FindAsync(id);
             if (tagToUpdate==null) return NotFound();
             tagToUpdate.Id = tag.Id;
             tagToUpdate.Name = tag.Name;
             await _context.SaveChangesAsync();
             return NoContent();
         }
         [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteTag(int id)
         {
             var tagToDelete = await _context.Tags.FindAsync(id);
             if (tagToDelete==null) return NotFound();
             _context.Tags.Remove(tagToDelete);
             await _context.SaveChangesAsync();
             return NoContent();
         }
     }
}
