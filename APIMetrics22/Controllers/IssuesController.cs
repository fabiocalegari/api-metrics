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
using System.Globalization;

namespace APIMetrics22.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class IssuesController : Controller
    {
        private readonly APIMetricsContext _context;
        private readonly ILogger _logger;
        private CultureInfo _culture;

        public IssuesController(APIMetricsContext context, ILogger<IssuesController> logger)
        {
            _context = context;
            _logger = logger;
            _culture = new CultureInfo("pt-BR");
        }

        // GET: Issues
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Issue.ToListAsync());
        }

        // GET: Issues/Details/EAC-2222
        [HttpGet("{key}")]
        public async Task<IActionResult> Details(string key)
        {
            if (key == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .FirstOrDefaultAsync(m => m.Key == key);

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

        //GET: Issues/Projects/EA Assinaturas/2018-01-01/2018-12-31
        [HttpGet("projects/{project}/{initDate}/{endDate}")]
        public async Task<IActionResult> Projects(string project, string initDate, string endDate)
        {
            if (project == null || initDate == null || endDate == null)
            {
                return NotFound();
            }

            var dtInitDate = Convert.ToDateTime(initDate, _culture);
            var dtEndDate = Convert.ToDateTime(endDate, _culture);

            //Return issues of a project with transitions EAGER
            var issues = await _context.Issue.Where(iss => iss.Project == project)
                    .Include(i => i.Transitions)
                    .Where(i => i.Transitions.Any(t => t.DataEvento >= dtInitDate && t.DataEvento <= dtEndDate && t.TransicaoDestino == "Done"))
                    .ToListAsync();

            return Ok(issues);
        }

    }
}
