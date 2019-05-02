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
        /// <summary>
        /// Lista todas as atividades com suas transições
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Retorna todas as atividaeds da base. Pode ser lento!</response>
        // GET: Issues
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Issue.ToListAsync());
        }
        /// <summary>
        /// Lista uma atividade específica com suas transições. A busca é feita através da chave. Ex.: EAC-2222
        /// </summary>
        /// <param name="key">É a chave da atividade. (Ex.: EAC-2222)</param>
        /// <returns></returns>
        /// <response code="200">Retorna a atividaed selecionada</response>
        // GET: Issues/Details/EAC-2222
        [HttpGet("{key}")]
        public async Task<IActionResult> Details(string key)
        {
            if (key == null)
            {
                return NotFound();
            }

            //EAGER Load of Transitions to compose Json
            var issue = await _context.Issue.Include(iss => iss.Transitions)
                .FirstOrDefaultAsync(m => m.Key == key);

            if (issue == null)
            {
                return NotFound();
            }

            return Ok(issue);
        }
        /// <summary>
        /// Lista todas as atividades de um projeto específico, onde o nome do projeto é o parâmetro. Ex.: EA Cobrança
        /// </summary>
        /// <param name="project">Os Projetos principais são: EA Assinaturas, EA Atendimento, EA Cobrança e EA Normandia.</param>
        /// <returns></returns>
        /// <response code="200">Retorna todas as atividades do projeto selecionado</response>
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
        /// <summary>
        /// Lista todas as atividades de um projeto específico em um determinado período. (Formato de data: AAAA-MM-DD)
        /// </summary>
        /// <param name="project">Os Projetos principais são: EA Assinaturas, EA Atendimento, EA Cobrança e EA Normandia.</param>
        /// <param name="initDate">Data de início para o filtro de acordo com a data de conclusão da atividade.</param>
        /// <param name="endDate">Data fim para o filtro de acordo com a data de conclusão da atividade.</param>
        /// <response code="200">Retorna todas as atividades do projeto no período selecionado com suas transições</response>
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
