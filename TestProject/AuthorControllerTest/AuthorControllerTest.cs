using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ApplicationBook.Authors.Commands.CreateAuthor;
using ApplicationBook.Authors.Commands.UpdateAuthor;
using ApplicationBook.Authors.Commands.DeleteAuthor;
using ApplicationBook.Authors.Queries.GetAllAuthors;
using ApplicationBook.Authors.Queries.GetAuthorById;
using Domain.Dtos;
using System.Collections.Generic;
using Domain;

namespace AuthorControllerTests
{
    [TestFixture]
    public class AuthorControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<AuthorController>> _loggerMock;
        private AuthorController _controller;
        

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AuthorController>>();
            _controller = new AuthorController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllAuthors_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var authors = new List<Author>
            {
            new Author { FirstName = "John", LastName = "Doe" },
            new Author { FirstName = "Joe", LastName = "Glenn" }
            };

            var operationResult = OperationResult<List<Author>>.Successfull(authors);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllAuthorsQuery>(), default))
                         .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetAllAuthors();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(authors, okResult.Value);
        }


        [Test]
        public async Task GetAuthorById_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = 1;

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetAuthorByIdQuery>(q => q.Id == authorId), CancellationToken.None))
                .ReturnsAsync(OperationResult<Author>.Failure("Author not found"));

            // Act
            var result = await _controller.GetAuthorById(authorId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result, "The result should be of type NotFoundObjectResult.");
            var notFoundResult = result as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult, "The NotFoundObjectResult should not be null.");
            Assert.AreEqual("Author not found", notFoundResult.Value, "The error message should match the expected value.");

            // Verify
            _mediatorMock.Verify(
                m => m.Send(It.Is<GetAuthorByIdQuery>(q => q.Id == authorId), CancellationToken.None),
                Times.Once
            );
        }


        [Test]
        public async Task CreateAuthor_ReturnsBadRequest_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateAuthorCommand(new AuthorDto ("", ""));

            // Act
            var result = await _controller.CreateAuthor(command);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Both FirstName and LastName must be provided.", badRequestResult.Value);
        }



        [Test]
        public async Task UpdateAuthor_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateCommand = new UpdateAuthorCommand(1, "UpdatedFirstName", "UpdatedLastName");

            _mediatorMock
                .Setup(m => m.Send(updateCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<Author>.Successfull(new Author { AId = 1, FirstName = "UpdatedFirstName", LastName = "UpdatedLastName" }));

            // Act
            var result = await _controller.UpdateAuthor(1, updateCommand);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task UpdateAuthor_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var updateCommand = new UpdateAuthorCommand(2, "UpdatedFirstName", "UpdatedLastName");

            // Act
            var result = await _controller.UpdateAuthor(1, updateCommand);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("The provided ID does not match the ID in the request body.", badRequestResult.Value);
        }

        [Test]
        public async Task UpdateAuthor_ReturnsBadRequest_WhenCommandIsInvalid()
        {
            // Arrange
            var updateCommand = new UpdateAuthorCommand(1, "", "");

            // Act
            var result = await _controller.UpdateAuthor(1, updateCommand);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Both FirstName and LastName are required.", badRequestResult.Value);
        }



        [Test]
        public async Task UpdateAuthor_ReturnsInternalServerError_WhenUpdateFails()
        {
            // Arrange
            var updateCommand = new UpdateAuthorCommand(1, "UpdatedFirstName", "UpdatedLastName");

            _mediatorMock
                .Setup(m => m.Send(updateCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<Author>.Failure("An unexpected error occurred."));

            // Act
            var result = await _controller.UpdateAuthor(1, updateCommand);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("An unexpected error occurred.", objectResult.Value);
        }

        [Test]
        public async Task DeleteAuthor_ReturnsOk_WhenDeletionIsSuccessful()
        {
            // Arrange
            var author = new Author { AId = 1, FirstName = "John", LastName = "Doe" };

            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteAuthorCommand>(cmd => cmd.Id == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<Author>.Successfull(author));

            // Act
            var result = await _controller.DeleteAuthor(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(author, okResult.Value);
        }

        [Test]
        public async Task DeleteAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteAuthorCommand>(cmd => cmd.Id == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<Author>.Failure("Author not found"));

            // Act
            var result = await _controller.DeleteAuthor(1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Author not found", notFoundResult.Value);
        }
    }
}


