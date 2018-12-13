using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APIMetrics22;
using APIMetrics22.Models;
using Microsoft.Extensions.Logging;

namespace APIMetrics22.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class IssuesController : Controller
    {
        private readonly APIMetricsContext _context;
        private readonly ILogger _logger;

        public IssuesController(APIMetricsContext context, ILogger<IssuesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Issues
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Issue.ToListAsync());
        }

        // GET: Issues/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .FirstOrDefaultAsync(m => m.Id == id);

            //EAGER Load to compose Json
            _context.Entry(issue).Collection(iss => iss.Transitions).Load();

            if (issue == null)
            {
                return NotFound();
            }

            return Ok(issue);
        }

        //GET: Issues/Projects/EA Assinaturas
        [HttpGet("projects/{project}")]
        public async Task<IActionResult> Projects(string project)
        {
            if (project == null)
            {
                return NotFound();
            }

            //Return issues of a project with transitions EAGER
            var issues = await _context.Issue.Where(iss => iss.Project == project).Include(i => i.Transitions).ToListAsync();

            return Ok(issues);
        }

    }
}
