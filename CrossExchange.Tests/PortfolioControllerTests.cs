using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace CrossExchange.Tests
{
    class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

        private readonly PortfolioController _portfolioController;

        public PortfolioControllerTests()
        {
            _portfolioController = new PortfolioController(_portfolioRepositoryMock.Object);
        }
        [Test]
        public async Task Post_ShouldInsertPortfolio()
        {
            var portfolio = new Portfolio
            {
                Name = "Test Portfolio"
            };

            // Arrange

            // Act
            var result = await _portfolioController.Post(portfolio);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }
        [Test]
        public async Task GetPortfolioInfo_ReturnsNotNull()
        {
            int portfolioId = 1;

            var portfolioRepositoryMock = new Mock<IPortfolioRepository>();

            portfolioRepositoryMock
                .Setup(x => x.GetAsync(It.Is<int>(id => id == portfolioId)))
                .Returns(Task.FromResult(new Portfolio() { Id = portfolioId, Name = "John Doe" }));

            var portfolioController = new PortfolioController(portfolioRepositoryMock.Object);

            var result = await portfolioController.GetPortfolioInfo(portfolioId) as OkObjectResult;

            Assert.NotNull(result);

            var resultPortfolio = result.Value as Portfolio;

            Assert.NotNull(resultPortfolio);

            Assert.AreEqual(portfolioId, resultPortfolio.Id);
        }
        [Test]
        public async Task GetPortfolioInfo()
        {
            var portfolioId = 1;

            // Arrange

            // Act
            var result = await _portfolioController.GetPortfolioInfo(portfolioId);

            // Assert
            Assert.NotNull(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
