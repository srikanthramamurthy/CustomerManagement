using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CustomerManagement.Api.Common.Exception;
using CustomerManagement.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerManagement.Api.Unit.Tests
{
    public class When_ExceptionHandlerMiddleware_Is_Used
    {
        public When_ExceptionHandlerMiddleware_Is_Used()
        {
            _context.Response.Body = new MemoryStream();
        }

        private readonly HttpContext _context = new DefaultHttpContext();
        private readonly Mock<RequestDelegate> _mockRequestDelegate = new Mock<RequestDelegate>();

        private readonly Mock<ILogger<ExceptionHandlerMiddleware>> _mockLogger =
            new Mock<ILogger<ExceptionHandlerMiddleware>>();

        [Fact]
        public async Task Then_It_Does_Nothing_When_There_Is_No_Exception()
        {
            _mockRequestDelegate.Setup(x => x.Invoke(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            var middleware = new ExceptionHandlerMiddleware(_mockRequestDelegate.Object);

            await middleware.InvokeAsync(_context, _mockLogger.Object);

            Assert.Equal(0, _context.Response.Body.Length);
            Assert.Equal((int) HttpStatusCode.OK, _context.Response.StatusCode);
        }

        [Fact]
        public async Task Then_It_Ignores_Non_Custom_Exceptions()
        {
            _mockRequestDelegate.Setup(x => x.Invoke(It.IsAny<HttpContext>())).ThrowsAsync(new ArgumentException());

            var middleware = new ExceptionHandlerMiddleware(_mockRequestDelegate.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => middleware.InvokeAsync(_context, _mockLogger.Object));
        }

        [Fact]
        public async Task Then_It_Should_Return_400_For_BadRequestException()
        {
            _mockRequestDelegate.Setup(x => x.Invoke(It.IsAny<HttpContext>())).ThrowsAsync(new BadRequestException());

            var middleware = new ExceptionHandlerMiddleware(_mockRequestDelegate.Object);

            await middleware.InvokeAsync(_context, _mockLogger.Object);

            Assert.NotEqual(0, _context.Response.Body.Length);
            Assert.Equal((int) HttpStatusCode.BadRequest, _context.Response.StatusCode);
        }


        [Fact]
        public async Task Then_It_Should_Return_404_For_NotFoundException()
        {
            _mockRequestDelegate.Setup(x => x.Invoke(It.IsAny<HttpContext>())).ThrowsAsync(new NotFoundException());

            var middleware = new ExceptionHandlerMiddleware(_mockRequestDelegate.Object);

            await middleware.InvokeAsync(_context, _mockLogger.Object);

            Assert.NotEqual(0, _context.Response.Body.Length);
            Assert.Equal((int) HttpStatusCode.NotFound, _context.Response.StatusCode);
        }


        [Fact]
        public async Task Then_It_Should_Return_422_For_UniqueEntityRuleException()
        {
            _mockRequestDelegate.Setup(x => x.Invoke(It.IsAny<HttpContext>()))
                .ThrowsAsync(new UniqueEntityRuleException());

            var middleware = new ExceptionHandlerMiddleware(_mockRequestDelegate.Object);

            await middleware.InvokeAsync(_context, _mockLogger.Object);

            Assert.NotEqual(0, _context.Response.Body.Length);
            Assert.Equal((int) HttpStatusCode.UnprocessableEntity, _context.Response.StatusCode);
        }
    }
}