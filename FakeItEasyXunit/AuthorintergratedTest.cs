using Xunit;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationBook.Authors.Queries.GetAllAuthors;
using Domain;
using Domain.Dtos;
using ApplicationBook.Authors.Queries.GetAuthorById;
using ApplicationBook.Authors.Commands.CreateAuthor;
using ApplicationBook.Authors.Commands.UpdateAuthor;
using ApplicationBook.Authors.Commands.DeleteAuthor;
using ApplicationBook.Interfaces.RepoInterfaces;

namespace FakeItEasyXunit
{
    public class AuthorintergratedTest
    {
        private readonly IMediator _mediatorFake; 
        private readonly ILogger<AuthorController> _loggerFake;
        private readonly AuthorController _controller;

        public AuthorintergratedTest()
        {
            // Skapa mocks för beroenden
            _mediatorFake = A.Fake<IMediator>(); // Skapa en fake av IMediator
            _loggerFake = A.Fake<ILogger<AuthorController>>();

            // Instansiera controllern med mocks
            _controller = new AuthorController(_mediatorFake, _loggerFake);
        }


        [Fact]
        public async Task GetAllAuthors_ReturnsOkWithAuthors_WhenAuthorsExist()
        {
            // Arrange
            var fakeAuthors = new List<Author>
            {
                new Author { FirstName = "Author 1", LastName = "EfterNamn" },
                new Author { FirstName = "Author 2", LastName = "EfterNamn" }
            };

            var operationResult = OperationResult<List<Author>>.Successfull(fakeAuthors);

            // Konfigurera fake-mediatorn för att returnera författare
            A.CallTo(() => _mediatorFake.Send(A<GetAllAuthorsQuery>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(operationResult));

            // Act
            var result = await _controller.GetAllAuthors();

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            Xunit.Assert.Equal(fakeAuthors, okResult.Value);
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsNotFound_WhenNoAuthorsExist()
        {
            // Arrange
            var operationResult = OperationResult<List<Author>>.Failure("No authors found.");

            A.CallTo(() => _mediatorFake.Send(A<GetAllAuthorsQuery>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(operationResult));

            // Act
            var result = await _controller.GetAllAuthors();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            // Använd reflection för att läsa egenskapen "Message"
            var value = notFoundResult.Value;
            var messageProperty = value.GetType().GetProperty("Message");
            var message = messageProperty?.GetValue(value, null);

            Assert.NotNull(message);
            Assert.Equal("No authors found.", message);
        }
        [Fact]
        public async Task GetAuthorById_ReturnsOk_WhenAuthorExists()
        {
            // Arrange
            var testAuthorId = 1;
            var testAuthor = new Author { AId = testAuthorId, FirstName = "John", LastName = "Doe" };

            var successResult = OperationResult<Author>.Successfull(testAuthor);

            // Konfigurera IMediator att returnera en författare
            A.CallTo(() => _mediatorFake.Send(A<GetAuthorByIdQuery>.That.Matches(q => q.Id == testAuthorId), A<CancellationToken>._))
                .Returns(Task.FromResult(successResult));

            // Act
            var result = await _controller.GetAuthorById(testAuthorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAuthor = Assert.IsType<Author>(okResult.Value);
            Assert.Equal(testAuthorId, returnedAuthor.AId);
            Assert.Equal("John", returnedAuthor.FirstName);
            Assert.Equal("Doe", returnedAuthor.LastName);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var testAuthorId = 999;

            var failureResult = OperationResult<Author>.Failure($"Author with ID {testAuthorId} was not found.");

            // Konfigurera IMediator att returnera ett misslyckat resultat
            A.CallTo(() => _mediatorFake.Send(A<GetAuthorByIdQuery>.That.Matches(q => q.Id == testAuthorId), A<CancellationToken>._))
                .Returns(Task.FromResult(failureResult));

            // Act
            var result = await _controller.GetAuthorById(testAuthorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Author with ID {testAuthorId} was not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = -1;

            var failureResult = OperationResult<Author>.Failure("The ID must be a positive integer.");

            // Konfigurera IMediator att returnera fel för ogiltigt ID
            A.CallTo(() => _mediatorFake.Send(A<GetAuthorByIdQuery>.That.Matches(q => q.Id == invalidId), A<CancellationToken>._))
                .Returns(Task.FromResult(failureResult));

            // Act
            var result = await _controller.GetAuthorById(invalidId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("The ID must be a positive integer.", notFoundResult.Value);
        }
        [Fact]
        public async Task CreateAuthor_ReturnsOk_WhenAuthorIsSuccessfullyCreated()
        {
            // Arrange
            var testAuthorDto = new AuthorDtoWithId(1, "Jane", "Doe");
            var command = new CreateAuthorCommand(new AuthorDto("Jane", "Doe"));

            // Skapa ett framgångsrikt resultat med AuthorDtoWithId
            var successResult = OperationResult<AuthorDtoWithId>.Successfull(testAuthorDto);

            // Mocka IMediator för att returnera ett lyckat resultat med AuthorDtoWithId
            A.CallTo(() => _mediatorFake.Send(command, A<CancellationToken>._))
                .Returns(Task.FromResult(successResult));

            // Act
            var result = await _controller.CreateAuthor(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Kontrollera att vi får en OkObjectResult
            var returnedAuthor = Assert.IsType<AuthorDtoWithId>(okResult.Value); // Kontrollera att resultatet är av typen AuthorDtoWithId
            Assert.Equal(testAuthorDto.Id, returnedAuthor.Id); // Kontrollera författarens ID
            Assert.Equal(testAuthorDto.FirstName, returnedAuthor.FirstName); // Kontrollera författarens förnamn
            Assert.Equal(testAuthorDto.LastName, returnedAuthor.LastName); // Kontrollera författarens efternamn
        }



        [Fact]
        public async Task CreateAuthor_ReturnsBadRequest_WhenCommandIsInvalid()
        {
            // Arrange
            var invalidCommand = new CreateAuthorCommand(new AuthorDto ("", ""));

            // Act
            var result = await _controller.CreateAuthor(invalidCommand);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Both FirstName and LastName must be provided.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsServerError_WhenCreationFails()
        {
            // Arrange
            var newAuthor = new AuthorDto("Jane", "Doe");
            var command = new CreateAuthorCommand(newAuthor);

            var failureResult = OperationResult<AuthorDtoWithId>.Failure("An error occurred while creating the author.");

            // Konfigurera IMediator att returnera ett misslyckat resultat
            A.CallTo(() => _mediatorFake.Send(command, A<CancellationToken>._))
                .Returns(Task.FromResult(failureResult));

            // Act
            var result = await _controller.CreateAuthor(command);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            Assert.Equal("An error occurred while creating the author.", serverErrorResult.Value);
        }
        [Fact]
        public async Task UpdateAuthor_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var command = new UpdateAuthorCommand(1, "UpdatedFirstName", "UpdatedLastName");
            var successResult = OperationResult<Author>.Successfull(new Author { AId = 1, FirstName = "UpdatedFirstName", LastName = "UpdatedLastName" });

            // Mocka IMediator för att returnera framgångsrik uppdatering
            A.CallTo(() => _mediatorFake.Send(command, A<CancellationToken>._))
                .Returns(successResult);

            // Act
            var result = await _controller.UpdateAuthor(1, command);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsBadRequest_WhenCommandBodyIsNull()
        {
            // Act
            var result = await _controller.UpdateAuthor(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The request body cannot be null.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsBadRequest_WhenIdMismatchOccurs()
        {
            // Arrange
            var command = new UpdateAuthorCommand(2, "FirstName", "LastName");

            // Act
            var result = await _controller.UpdateAuthor(1, command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The provided ID does not match the ID in the request body.", badRequestResult.Value);
        }

        

        [Fact]
        public async Task UpdateAuthor_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var command = new UpdateAuthorCommand(1, "FirstName", "LastName");
            var failureResult = OperationResult<Author>.Failure("An error occurred while updating the author.");

            // Mocka IMediator för att returnera ett serverfel
            A.CallTo(() => _mediatorFake.Send(command, A<CancellationToken>._))
                .Returns(failureResult);

            // Act
            var result = await _controller.UpdateAuthor(1, command);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            Assert.Equal("An error occurred while updating the author.", serverErrorResult.Value);
        }
        [Fact]
        public async Task DeleteAuthor_ReturnsOk_WhenDeleteIsSuccessful()
        {
            // Arrange
            var command = new DeleteAuthorCommand(1);
            var deletedAuthor = new Author { AId = 1, FirstName = "John", LastName = "Doe" };
            var successResult = OperationResult<Author>.Successfull(deletedAuthor);
            

            // Mocka IMediator för att returnera en framgångsrik borttagning
            //A.CallTo(() => _mediatorFake.Send(command, A<CancellationToken>._))
            //    .Returns(successResult);
            A.CallTo(() => _mediatorFake.Send(
                A<DeleteAuthorCommand>.That.Matches(c => c.Id == 1),
                A<CancellationToken>._))
                .Returns(successResult);

            // Act
            var result = await _controller.DeleteAuthor(1);


            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Kontrollera att vi får en OkObjectResult
            var returnedAuthor = Assert.IsType<Author>(okResult.Value);  // Kontrollera att resultatet är av typen Author
            Assert.Equal(deletedAuthor.AId, returnedAuthor.AId);  // Kontrollera att ID:n matchar
            Assert.Equal("John", returnedAuthor.FirstName);  // Kontrollera FirstName
            Assert.Equal("Doe", returnedAuthor.LastName);  // Kontrollera LastName
        }


        [Fact]
        public async Task DeleteAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = 100;
            var command = new DeleteAuthorCommand(authorId);
            var failureResult = OperationResult<Author>.Failure($"Author with ID {authorId} not found.");

            // Mocka IMediator för att returnera att författaren inte hittades
            A.CallTo(() => _mediatorFake.Send(A<DeleteAuthorCommand>._, A<CancellationToken>._))
                .Returns(failureResult);

            // Act
            var result = await _controller.DeleteAuthor(authorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            // Kontrollera att värdet i NotFoundObjectResult matchar förväntad felmeddelande
            Assert.NotNull(notFoundResult.Value);
            var errorMessage = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal($"Author with ID {authorId} not found.", errorMessage);
        }





    }

}



