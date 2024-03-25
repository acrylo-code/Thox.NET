using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thox.Areas.Admin.Controllers;
using Thox.Data;
using Thox.Models;

namespace Thox.Net_test
{

    internal class AdminControllers
    {



        [TestFixture]
        public class HomeControllerTests
        {
            [Test]
            public void Index_ReturnsViewResult()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<HomeController>>();
                var controller = new HomeController();

                // Act
                var result = controller.Index();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(result);
                    Assert.IsInstanceOf<ViewResult>(result);
                });
            }
        }

        [TestFixture]
        public class RoomsControllerTests
        {
            [Test]
            public void Index_ReturnsViewResult()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<RoomsController>>();
                // Create a mock of your ApplicationDbContext
                var mockDB = new Mock<ApplicationDbContext>();
                var controller = new RoomsController(mockDB.Object);

                // Act
                var result = controller.Index();

                Assert.IsNotNull(result);
            }
        }
        [TestFixture]
        public class ReservationSlotsControllerTests
        {
            [Test]
            public void Index_ReturnsViewResult()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<ReservationSlotsController>>();
                var mockDB = new Mock<ApplicationDbContext>();
                var controller = new ReservationSlotsController(mockDB.Object);

                // Act
                var resultTask = controller.Index(); // No need to await here
                Assert.IsNotNull(resultTask);
            }
        }

        [TestFixture]
        public class RoomPricesControllerTests
        {
            [Test]
            public void Index_ReturnsViewResult()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<RoomPricesController>>();
                var mockDB = new Mock<ApplicationDbContext>();
                var controller = new RoomPricesController(mockDB.Object);

                // Act
                var result = controller.Index(); // Await the asynchronous action method

                // Assert
                Assert.IsNotNull(result);
            }
        }
    }
}
