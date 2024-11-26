using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationBook.Authors.Commands.CreateAuthor;
using ApplicationBook.Authors.Commands.UpdateAuthor;
using ApplicationBook.Authors.Commands.DeleteAuthor;
using ApplicationBook.Authors.Queries.GetAllAuthors;
using ApplicationBook.Authors.Queries.GetAuthorById;
using Domain;
using MediatR;


namespace ApplicationBook.Tests.Controllers
{
    [TestFixture]
    public class AuthorControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private AuthorController _controller;




        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthorController(_mediatorMock.Object);
        }

        [Test]
        public async Task GetAuthors_ShouldReturnOkResult_WithListOfAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {

                new Author(1, "Toby", "Goransson"),
                new Author(2, "Toby2", "Goransson2"),
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllAuthorsQuery>(), default))
                .ReturnsAsync(authors);

            // Act
            var result = await _controller.GetAuthors();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(authors, okResult.Value);
        }
        [Test]
        public async Task GetAuthorById_ShouldReturnAuthor_WhenAuthorExists()
        {
            // Arrange
            var authorId = 1;
            var existingAuthor = new Author(authorId, "John", "Doe");

            
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetAuthorByIdQuery>(q => q.Id == authorId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAuthor);

            // Act
            var result = await _controller.GetAuthorById(authorId);

            // Assert
            Assert.IsNotNull(result); 
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult); 
            Assert.AreEqual(200, okResult.StatusCode); 
            var returnedAuthor = okResult.Value as Author;
            Assert.IsNotNull(returnedAuthor); 
            Assert.AreEqual(existingAuthor.Id, returnedAuthor.Id);
        }

        [Test]
        public async Task GetAuthorById_ShouldReturnNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = 99; 
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetAuthorByIdQuery>(q => q.Id == authorId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author)null); 
            // Act
            var result = await _controller.GetAuthorById(authorId);

            // Assert
            Assert.IsNotNull(result); 
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult); 
            Assert.AreEqual(404, notFoundResult.StatusCode); 

            
            if (notFoundResult.Value is { } notFoundMessage)
            {
                var message = notFoundMessage.ToString();
                Assert.IsTrue(message.Contains("was not found")); 
            }
            else
            {
                Assert.Fail("Expected a message in the NotFoundObjectResult.");
            }
        }





        [Test]
        public async Task CreateAuthor_ShouldReturnCreated_WhenValidAuthorIsProvided()
        {
            // Arrange
            var command = new CreateAuthorCommand("John", "Doe");
            var newAuthor = new Author(1, "John", "Doe");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newAuthor);

            // Act
            var result = await _controller.CreateAuthor(command);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual(newAuthor, createdAtActionResult.Value);
        }

        [Test]
        public async Task CreateAuthor_ShouldReturnBadRequest_WhenFirstNameOrLastNameIsMissing()
        {
            // Arrange
            var command = new CreateAuthorCommand("", ""); 

            // Act
            var result = await _controller.CreateAuthor(command);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Both FirstName and LastName must be provided.", badRequestResult.Value);
        }

        [Test]
        public async Task CreateAuthor_ShouldReturnBadRequest_WhenCommandIsNull()
        {
            // Act
            var result = await _controller.CreateAuthor(null);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("The request body cannot be null.", badRequestResult.Value);
        }

        [Test]
        public async Task CreateAuthor_ShouldReturnInternalServerError_WhenAuthorCreationFails()
        {
            // Arrange
            var command = new CreateAuthorCommand("John", "Doe");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author)null); 

            // Act
            var result = await _controller.CreateAuthor(command);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("An error occurred while creating the author.", objectResult.Value);
        }


        [Test]
        public async Task UpdateAuthor_ShouldReturnNoContent_WhenValidAuthorIsProvided()
        {
            // Arrange
            var authorId = 1;
            var command = new UpdateAuthorCommand(authorId, "John", "Doe");
            var updatedAuthor = new Author(authorId, "John", "Doe");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedAuthor);

            // Act
            var result = await _controller.UpdateAuthor(authorId, command);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }


        [Test]
        public async Task UpdateAuthor_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var authorId = 1;
            var command = new UpdateAuthorCommand(2, "John", "Doe"); 

            // Act
            var result = await _controller.UpdateAuthor(authorId, command);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("The provided ID does not match the ID in the request body.", badRequestResult.Value);
        }

        [Test]
        public async Task UpdateAuthor_ShouldReturnBadRequest_WhenFirstNameOrLastNameIsMissing()
        {
            // Arrange
            var command = new UpdateAuthorCommand(1, "", ""); 
            // Act
            var result = await _controller.UpdateAuthor(1, command);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Both FirstName and LastName are required.", badRequestResult.Value);
        }





        [Test]
        public async Task DeleteAuthor_ShouldReturnNoContent_WhenAuthorIsDeleted()
        {
            // Arrange
            var authorId = 1;
            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteAuthorCommand>(cmd => cmd.Id == authorId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author(authorId, "John", "Doe")); 
            // Act
            var result = await _controller.DeleteAuthor(authorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteAuthor_ShouldReturnNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = 99;
            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteAuthorCommand>(cmd => cmd.Id == authorId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author)null); 

            // Act
            var result = await _controller.DeleteAuthor(authorId);

            // Assert
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            
            
        }

        [Test]
        public async Task DeleteAuthor_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = -1; 

            // Act
            var result = await _controller.DeleteAuthor(invalidId);

            // Assert
            Assert.IsNotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid ID. ID must be a positive integer.", badRequestResult.Value);
        }
    }
}

