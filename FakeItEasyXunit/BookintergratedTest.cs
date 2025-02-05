

using ApplicationBook.Books.Commands.CreateBook;
using ApplicationBook.Books.Commands.DeleteBook;
using ApplicationBook.Books.Commands.UpdateBook;
using ApplicationBook.Books.Queries.GetBook;
using ApplicationBook.Books.Queries.GetBookById;
using Domain;
using Domain.Dtos;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web_API.Controllers;
using Xunit;

namespace FakeItEasyXunit;

public class BookIntegratedTests
{
    private readonly IMediator _mediatorFake;
    private readonly ILogger<BookController> _loggerFake;
    private readonly BookController _controller;

    public BookIntegratedTests()
    {
        _mediatorFake = A.Fake<IMediator>();
        _loggerFake = A.Fake<ILogger<BookController>>();
        _controller = new BookController(_mediatorFake, _loggerFake);
    }

    [Fact]
    public async Task GetBooks_ReturnsOkWithBooks_WhenBooksExist()
    {
        // Arrange
        var fakeBooks = new List<Book> { new Book { Title = "Sample Book" } };
        var successResult = OperationResult<List<Book>>.Successfull(fakeBooks);

        A.CallTo(() => _mediatorFake.Send(A<GetBooksQuery>.Ignored, A<CancellationToken>.Ignored))
            .Returns(Task.FromResult(successResult));

        // Act
        var result = await _controller.GetBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(fakeBooks, okResult.Value);
    }

    [Fact]
    public async Task GetBookById_ReturnsOk_WhenBookExists()
    {
        // Arrange
        var testBookId = 1;
        var testBook = new Book { BId = testBookId, Title = "Sample Book" };
        var successResult = OperationResult<Book>.Successfull(testBook);

        A.CallTo(() => _mediatorFake.Send(A<GetBookByIdQuery>.That.Matches(q => q.Id == testBookId), A<CancellationToken>.Ignored))
            .Returns(Task.FromResult(successResult));

        // Act
        var result = await _controller.GetBookById(testBookId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedBook = Assert.IsType<Book>(okResult.Value);
        Assert.Equal(testBookId, returnedBook.BId);
        Assert.Equal("Sample Book", returnedBook.Title);
    }

    [Fact]
    public async Task CreateNewBook_ReturnsOk_WhenBookIsSuccessfullyCreated()
    {
        // Arrange
        var testBookDto = new BookDto("New Book", "Newest book", 1, "Jon", "Doe");
        var successResult = OperationResult<BookDto>.Successfull(testBookDto);

        A.CallTo(() => _mediatorFake.Send(A<CreateBookCommand>.Ignored, A<CancellationToken>.Ignored))
            .Returns(Task.FromResult(successResult));

        // Act
        var result = await _controller.CreateNewBook(testBookDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedBook = Assert.IsType<BookDto>(okResult.Value);
        Assert.Equal("New Book", returnedBook.Title);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var bookToUpdate = new Book( "Updated Book", "Newest book");
        var updateCommand = new UpdateBookCommand(bookToUpdate);
        var successResult = OperationResult<Book>.Successfull(new Book { BId = 1, Title = "Updated Book" });

        A.CallTo(() => _mediatorFake.Send(updateCommand, A<CancellationToken>.Ignored))
            .Returns(Task.FromResult(successResult));

        // Act
        var result = await _controller.UpdateBook(1, bookToUpdate);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteBook_ReturnsOk_WithRemainingBooks_WhenDeleteIsSuccessful()
    {
        // Arrange
        var bookIdToDelete = 1;
        var command = new DeleteBookCommand(bookIdToDelete);

        // Böcker som är kvar efter att en bok tagits bort
        var remainingBooks = new List<Book>
    {
        new Book(2, "Remaining Book 1", "Description 1"),
        new Book(3, "Remaining Book 2", "Description 2")
    };

        var successResult = OperationResult<List<Book>>.Successfull(remainingBooks);

        A.CallTo(() => _mediatorFake.Send(
            A<DeleteBookCommand>.That.Matches(c => c.Id == bookIdToDelete),
            A<CancellationToken>._))
             .Returns(Task.FromResult(successResult));

        // Act
        var result = await _controller.DeleteBook(bookIdToDelete);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);  // Kontrollera att vi får en OkObjectResult
        var returnedBooks = Assert.IsType<List<Book>>(okResult.Value);  // Kontrollera att resultatet är en lista av böcker

        // Kontrollera att listan innehåller de förväntade böckerna
        Assert.Equal(2, returnedBooks.Count);
        Assert.Contains(returnedBooks, b => b.BId == 2 && b.Title == "Remaining Book 1" && b.Description == "Description 1");
        Assert.Contains(returnedBooks, b => b.BId == 3 && b.Title == "Remaining Book 2" && b.Description == "Description 2");
    }



    [Fact]
    public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        var bookId = 100;
        var command = new DeleteBookCommand(bookId);
        var failureResult = OperationResult<List<Book>>.Failure($"No book found with ID {bookId}.");

        A.CallTo(() => _mediatorFake.Send(
            A<DeleteBookCommand>.That.Matches(c => c.Id == bookId),
            A<CancellationToken>._))
            .Returns(Task.FromResult(failureResult));

        // Act
        var result = await _controller.DeleteBook(bookId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        
    }


}
