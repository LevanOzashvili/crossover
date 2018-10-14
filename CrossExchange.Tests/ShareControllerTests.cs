using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;

namespace CrossExchange.Tests
{
    public class ShareControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly ShareController _shareController;

        public ShareControllerTests()
        {
            _shareController = new ShareController(_shareRepositoryMock.Object);
        }

        [Test]
        public async Task Post_ShouldInsertHourlySharePrice()
        {
            var hourRate = new HourlyShareRate
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };

            // Arrange

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }
        //[Test]
        //public async Task GetShares()
        //{
        //    var symbol = "CBI";

        //    // Arrange

        //    // Act
        //    var result = await _shareController.Get(symbol) as OkObjectResult;

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.AreEqual(200, result.StatusCode);
        //    var resultList = result.Value as List<HourlyShareRate>;

        //    Assert.NotNull(resultList);

        //    Assert.NotZero(resultList.Count);
        //}
        //[Test]
        //public async Task GetLatestPrice()
        //{
        //    var symbol = "CBI";

        //    // Arrange

        //    // Act
        //    var result =  _shareController.GetLatestPrice(symbol) as OkObjectResult;

        //    // Assert
        //    Assert.NotNull(result);

        //    var okResult = result as OkObjectResult;
        //    Assert.NotNull(okResult);
        //    Assert.AreEqual(200, okResult.StatusCode);
        //}

    }
}
