using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossExchange.Controller
{
    [Route("api/Share")]
    public class ShareController : ControllerBase
    {
        public IShareRepository _shareRepository { get; set; }

        public ShareController(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get([FromQuery(Name = "symbol")]string symbol)
        {
            //var shares = _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).ToList();
            var shares = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).ToListAsync();
            return Ok(shares);
        }


        [HttpGet("Latest/{symbol}")]
        public async Task<IActionResult> GetLatestPrice([FromQuery(Name = "symbol")]string symbol)
        {
            
            //var share = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).FirstOrDefaultAsync();
            var share = (await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).ToListAsync())
                                .OrderByDescending(x => x.TimeStamp).FirstOrDefault();
            return Ok(share?.Rate);
           
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Post([FromBody]HourlyShareRate value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (value.Symbol.Length != 3) {
                return BadRequest("Symbol length must be 3");
            }
            await _shareRepository.InsertAsync(value);

            return Created($"Share/{value.Id}", value);
        }
        
    }
}
