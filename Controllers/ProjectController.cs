using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPApp.Data;
using ERPApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERPApp.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly AppDBContext _db;
        public ProjectController(AppDBContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "LoggedInRequired")]
        public IActionResult GetProjects()
        {
            var projects = _db.Projects.ToList();
            if (projects.Count == 0)
                return Content("No projects in db");

            return Ok(projects);
        }

        [HttpPost("addproject")]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> AddProject([FromBody] ProjectModel formData)
        {
            var newProj = new ProjectModel
            {
                Name = formData.Name,
                Number = formData.Number,
                Type = formData.Type,
                DateAdded = DateTime.Now
            };

            var projects = _db.Projects.Any(x => x.Number == formData.Number);
            if(projects)
                return Ok(new { messsage = "Project with this number already exist." });

            await _db.Projects.AddAsync(newProj);
            await _db.SaveChangesAsync();
            return Ok(new { messsage = "New project added." });
        }

        [HttpPut("updateproject/{id}")]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> UpdateProject([FromRoute] int id, [FromBody] ProjectModel formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var findProj = _db.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (findProj == null)
                return NotFound();

            findProj.Name = formData.Name;
            findProj.Number = formData.Number;
            findProj.Type = formData.Type;

            _db.Entry(findProj).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            return Ok(new JsonResult($"Project with name: {formData.Name} and id: {id} was updated."));
        }

        [HttpDelete("deleteproject/{id}")]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var findProj = await _db.Projects.FindAsync(id);
            if (findProj == null)
                return NotFound();

            _db.Projects.Remove(findProj);
            await _db.SaveChangesAsync();

            return Ok(new JsonResult($"Project with id: {id} was DELETED."));
        }

    }
}