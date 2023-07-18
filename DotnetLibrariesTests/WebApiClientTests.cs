using NUnit.Framework;
using Moq;
using Moq.Protected;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using DotnetLibraries;
using RichardSzalay.MockHttp;
using System;

namespace DotnetLibrariesTests
{
    public class WebApiClientTests
    {
        [Test]
        public async Task TestGetAsync()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
               })
               .Verifiable();

            var webApiClient = new WebApiClient(handlerMock.Object);
            //webApiClient.BaseAddress = new Uri("http://localhost/api/user/");

            // Act
            var response = await webApiClient.GetAsync("testuri");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        // https://stackoverflow.com/questions/36425008/mocking-httpclient-in-unit-tests
        [Test]
        public async Task TestGetAsyncV2()
        {
            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("http://localhost/api/user/*")
                    .Respond("application/json", "{'name' : 'Test McGee'}"); // Respond with JSON

            var webApiClient = new WebApiClient(mockHttp);
            webApiClient.BaseAddress = new Uri("http://localhost/api/user/");

            // Act
            var response = await webApiClient.GetAsync("testuri");

            var json = await response.Content.ReadAsStringAsync();

            // No network connection required
            Console.Write(json); // {'name' : 'Test McGee'}

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}