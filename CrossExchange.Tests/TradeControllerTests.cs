using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;

namespace CrossExchange.Tests
{
    class TradeControllerTests
    {
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();

        private readonly TradeController _tradeController;

        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

        private readonly PortfolioController _portfolioController;

        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly ShareController _shareController;
        public TradeControllerTests()
        {
            _tradeController = new TradeController(_shareRepositoryMock.Object,_tradeRepositoryMock.Object, _portfolioRepositoryMock.Object);
        }

        //[Test]
        //public async Task Get_ShouldReturnAllTradings()
        //{
        //    var portfolioId = 1;
        //    var result = await _tradeController.GetAllTradings(portfolioId);
            

        //    Assert.NotNull(result);
        //    var okResult = result as OkObjectResult;
        //    var resultList = okResult.Value as List<Trade>;

        //    Assert.NotNull(result);

        //    Assert.NotZero(resultList.Count);
        //}

        //[Test]
        //public async Task Post_ShouldInsertBuyTrade()
        //{
        //    var tradeModel = new TradeModel
        //    {
        //        Symbol = "CBI",
        //        NoOfShares = 1,
        //        PortfolioId = 1,
        //        Action = "BUY"
        //    };

        //    // Arrange

        //    // Act
        //    var result = await _tradeController.Post(tradeModel);

        //    // Assert
        //    Assert.NotNull(result);

        //    var createdResult = result as CreatedResult;
        //    Assert.NotNull(createdResult);
        //    Assert.AreEqual(201, createdResult.StatusCode);
        //}
        //[Test]
        //public async Task Post_ShouldInsertSellTrade()
        //{
        //    var tradeModel = new TradeModel
        //    {
        //        Symbol = "CBI",
        //        NoOfShares = 1,
        //        PortfolioId = 1,
        //        Action = "SELL"
        //    };

        //    // Arrange

        //    // Act
        //    var result = await _tradeController.Post(tradeModel);

        //    // Assert
        //    Assert.NotNull(result);

        //    var createdResult = result as CreatedResult;
        //    Assert.NotNull(createdResult);
        //    Assert.AreEqual(201, createdResult.StatusCode);
        //}
    }
}
