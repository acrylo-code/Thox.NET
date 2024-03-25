using Thox;
using Thox.Data;
using System;
using Thox.Models;
using Thox.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using static Thox.Mail.Mail;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Thox.Hubs;
using System.Dynamic;


namespace Thox.Net_test
{
    public class Tests
    {

        [TestFixture]
        public class HomeControllerTests
        {
            [Test]
            public void Index_ReturnsViewResult()
            {
				// Arrange
				var mockDB = new Mock<ApplicationDbContext>();
				var loggerMock = new Mock<ILogger<HomeController>>();
                var controller = new HomeController(loggerMock.Object, mockDB.Object);

                // Act
                var result = controller.Index();
                var result2 = controller.Privacy();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(result);
                    Assert.IsInstanceOf<ViewResult>(result);
                    Assert.IsNotNull(result2);
                    Assert.IsInstanceOf<ViewResult>(result2);
                });
            }
            [Test]
            public void Index_UsesLogger()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<HomeController>>();
				var mockDB = new Mock<ApplicationDbContext>();
				var controller = new HomeController(loggerMock.Object, mockDB.Object);

                // Act
                var result = controller.Index();

                // Assert
                loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(), // Match any log message
                        null,
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                    ),
                    Times.Once
                );
            }
        }
        [TestFixture]
        public class ReservationControllerTests
        {
            [Test]
            public void Index_ReturnsViewResult()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<ReservationController>>();
                var controller = new ReservationController();

                // Act
                var result = controller.Index();
                var result2 = controller.ReservationComplete();
                var result3 = controller.DateSelection(5);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(result);
                    Assert.IsInstanceOf<ViewResult>(result);
                    Assert.IsNotNull(result2);
                    Assert.IsInstanceOf<ViewResult>(result2);
                    Assert.IsNotNull(result3);
                    Assert.IsInstanceOf<ViewResult>(result3);
                });
            }
        }
        [TestFixture]
        public class ContactsControllerTests
        {
            [Test]
            public void Index_ReturnsViewResult()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<ContactController>>();
                var controller = new ContactController(loggerMock.Object);

                // Act
                var result = controller.me();
                var result2 = controller.form();


                // Assert
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(result);
                    Assert.IsInstanceOf<ViewResult>(result);
                    Assert.IsNotNull(result2);
                    Assert.IsInstanceOf<ViewResult>(result2);
                });
            }
        }


        [Test]
         public async Task SubmitContactForm_InvalidRecaptcha_ReturnsBadRequest()
         {
             // Arrange
             var formData = new FormData
             {
                 FirstName = "John",
                 LastName = "Doe",
                 Email = "john.doe@example.com",
                 Phone = "1234567890",
                 Message = "Test message",
                 RecaptchaToken = "valid_token"
             };
             var controller = new APIController();

             // Act
             var result = await controller.SubmitContactForm(formData);

             // Assert
             Assert.IsInstanceOf<BadRequestObjectResult>(result);
             var badRequestResult = result as BadRequestObjectResult;
             Assert.IsNotNull(badRequestResult);
         }
    }

}
