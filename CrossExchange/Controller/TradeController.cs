using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CrossExchange.Controller
{
    [Route("api/Trade")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _shareRepository = shareRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
            

        }


        [HttpGet("{portfolioId}")]
        public async Task<IActionResult> GetAllTradings([FromQuery(Name = "portfolioId")]int portfolioId)
        {
            //var trade =  _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portfolioId));
            var trade = await _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portfolioId)).ToListAsync();
            return Ok(trade);
        }



        /*************************************************************************************************************************************
        For a given portfolio, with all the registered shares you need to do a trade which could be either a BUY or SELL trade. For a particular trade keep following conditions in mind:
		BUY:
        a) The rate at which the shares will be bought will be the latest price in the database.
		b) The share specified should be a registered one otherwise it should be considered a bad request. 
		c) The Portfolio of the user should also be registered otherwise it should be considered a bad request. 
                
        SELL:
        a) The share should be there in the portfolio of the customer.
		b) The Portfolio of the user should be registered otherwise it should be considered a bad request. 
		c) The rate at which the shares will be sold will be the latest price in the database.
        d) The number of shares should be sufficient so that it can be sold. 
        Hint: You need to group the total shares bought and sold of a particular share and see the difference to figure out if there are sufficient quantities available for SELL. 

        *************************************************************************************************************************************/

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Post([FromBody]TradeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var portfilio = await _portfolioRepository.GetAsync(model.PortfolioId);
            if (portfilio == null)
            {
                return BadRequest("Invalid Portfolio " + model.PortfolioId.ToString());
            }
            var LastRate = (await _shareRepository.Query().Where(x => x.Symbol.Equals(model.Symbol)).ToListAsync())
                                .OrderByDescending(x => x.TimeStamp).FirstOrDefault().Rate;
            if (model.Action.Equals("BUY"))
            {
                var share = (await _shareRepository.Query().Where(x => x.Symbol.Equals(model.Symbol)).ToListAsync()).FirstOrDefault();
                if (share == null)
                {
                    return BadRequest("Invalid Symbol " + model.Symbol);
                }
            }
            else
            if (model.Action.Equals("SELL"))
            {
                var bought = (await _tradeRepository.Query().Where(x => x.PortfolioId.Equals(model.PortfolioId) && x.Symbol.Equals(model.Symbol) && x.Action.Equals("BUY")).ToListAsync())
                            .Sum(x => x.NoOfShares);
                var sold = (await _tradeRepository.Query().Where(x => x.PortfolioId.Equals(model.PortfolioId) && x.Symbol.Equals(model.Symbol) && x.Action.Equals("SELL")).ToListAsync())
                            .Sum(x => x.NoOfShares);
                var remainingQuantity = bought - sold;
                if (remainingQuantity <= 0 || remainingQuantity < model.NoOfShares)
                {
                    return BadRequest("Insufficient Quantity");
                }

            }
            var trade = new Trade()
            {
                Symbol = model.Symbol,
                NoOfShares = model.NoOfShares,
                Price = LastRate * model.NoOfShares,
                PortfolioId = model.PortfolioId,
                Action = model.Action,
            };

            await _tradeRepository.InsertAsync(trade);
            return Created($"Trade/{trade.Id}", trade);
        }
        
    }
}
