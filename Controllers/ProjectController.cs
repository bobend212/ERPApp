using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetProjects()
        {
            var projects = _db.Projects.ToList();
            if (projects.Count == 0)
                return Content("No projects in db");

            return Ok(projects);
        }

    }
}