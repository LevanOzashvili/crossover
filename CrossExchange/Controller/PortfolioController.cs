using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CrossExchange.Controller
{
    [Route("api/Portfolio")]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioRepository _portfolioRepository { get; set; }

        public PortfolioController(/*IShareRepository shareRepository, ITradeRepository tradeRepository, */ IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet("{portfolioid}")]
        public async Task<IActionResult> GetPortfolioInfo([FromQuery (Name = "portfolioid")]int portfolioid)
        {
            //var portfolio = _portfolioRepository.GetAll().Where(x => x.Id.Equals(portFolioid));
            var portfolio = await _portfolioRepository.GetAsync(portfolioid);
            return Ok(portfolio);
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Post([FromBody]Portfolio value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _portfolioRepository.InsertAsync(value);

            return Created($"Portfolio/{value.Id}", value);

        }

    }
}
