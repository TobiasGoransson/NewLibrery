using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Web_API.Controllers;
using ApplicationBook.Books.Commands.CreateBook;
using ApplicationBook.Books.Commands.DeleteBook;
using ApplicationBook.Books.Commands.UpdateBook;
using ApplicationBook.Books.Queries.GetBook;
using ApplicationBook.Books.Queries.GetBookById;

namespace Web_API.Tests.Controllers
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private BookController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BookController(_mediatorMock.Object);
        }

        [Test]
        public async Task Get_ShouldReturnOkResult_WithListOfBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book (1, "BookTest","TestOfBook",new Author(1, "Toby", "Goransson")),
                new Book (2, "BookTest2","TestOfBook2",new Author(2, "Toby2", "Goransson2"))
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllValuesQuery>(), default))
                .ReturnsAsync(books);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(books, okResult.Value);
        }

        [Test]
        public async Task GetById_ShouldReturnOkResult_WithSingleBook()
        {
            // Arrange
            var book = new Book(2, "BookTest2", "TestOfBook2", new Author(2, "Toby2", "Goransson2"));



            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetValueByIdQuery>(), default))
                .ReturnsAsync(book);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(book, okResult.Value);
        }
        [Test]
        public async Task Get_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await _controller.Get(invalidId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            
        }
        [Test]
        public async Task Get_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var nonExistentId = 99;

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetValueByIdQuery>(q => q.Id == nonExistentId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _controller.Get(nonExistentId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

           
        }
        [Test]
        public async Task Get_ShouldReturnOk_WhenBookExists()
        {
            // Arrange
            var existingId = 1;
            var book = new Book (1,"Test Book", "BookByToby", new Author (1,"Toby", "Gsson"));

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetValueByIdQuery>(q => q.Id == existingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            // Act
            var result = await _controller.Get(existingId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBook = okResult.Value as Book;
            Assert.IsNotNull(returnedBook);
            Assert.AreEqual(existingId, returnedBook.Id);
            Assert.AreEqual("Test Book", returnedBook.Title);
        }
        [Test]
        public async Task UpdateBook_ShouldReturnOk_WhenBookIsUpdated()
        {
            // Arrange
            var bookIdToUpdate = 1;
            var updatedBook = new Book(bookIdToUpdate, "Updated Title", "Updated Description", new Author(1, "Toby", "Goransson"));

            // Simulera att boken finns och att den uppdateras
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), default))
                .ReturnsAsync(updatedBook);

            // Act
            var result = await _controller.UpdateBook(bookIdToUpdate, updatedBook);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result); 
            var okResult = result as OkObjectResult;
            Assert.AreEqual(updatedBook, okResult.Value); 
        }

        [Test]
        public async Task UpdateBook_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var bookIdToUpdate = 999; // Ett ID som inte finns
            var updatedBook = new Book(bookIdToUpdate, "Updated Title", "Updated Description", new Author(1, "Toby", "Goransson"));

            // Simulera att boken inte finns
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), default))
                .ThrowsAsync(new KeyNotFoundException($"Ingen bok hittades med ID {bookIdToUpdate}."));

            // Act
            var result = await _controller.UpdateBook(bookIdToUpdate, updatedBook);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result); 
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("{ Message = Ingen bok hittades med ID 999. }", notFoundResult.Value?.ToString()); 
        }

        [Test]
        public async Task UpdateBook_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var book = new Book(2, "BookTest2", "TestOfBook2", new Author(2, "Toby2", "Goransson2"));

            // Act
            var result = await _controller.UpdateBook(1, book);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task DeleteBook_ShouldReturnNoContent_WhenBookIsDeleted()
        {
            // Arrange
            var bookIdToDelete = 1;

            // Simulera att DeleteBookCommand returnerar en lista utan den borttagna boken
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteBookCommand>(), default))
                .ReturnsAsync(new List<Book>
                {
                new Book(2, "BookTest2", "TestOfBook2", new Author(2, "Toby2", "Goransson2"))
                });

            // Act
            var result = await _controller.DeleteBook(bookIdToDelete);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result); // Verifiera att resultatet är NoContent (HTTP 204)
        }

        [Test]
        public async Task DeleteBook_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var bookIdToDelete = 999; // Ett ID som inte finns i listan

            // Simulera att DeleteBookCommand returnerar en lista där boken inte finns
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteBookCommand>(), default))
                .ReturnsAsync((List<Book>)null); // Simulerar att inget resultat hittas

            // Act
            var result = await _controller.DeleteBook(bookIdToDelete);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result); // Verifiera att resultatet är NotFound (HTTP 404)
        }
        [Test]
        public async Task CreateNewBook_ShouldAddBook_WhenValidBookIsProvided_And_return_List_Including_NewBook()
        {
            // Arrange
            var newBook = new Book(0, "New Book", "Description of New Book", new Author(1, "Toby", "Goransson"));
           
            //Setup: Simulera att CreateBookCommand läggs till och returnera den uppdaterade listan med böcker
            var booksListWhenUpdated = new List<Book>
                {
                 new Book(1, "TobyBook", "Book of Toby", new Author(1, "Toby", "Goransson")),
                 new Book(2, "TobyBook2", "Book of Toby2", new Author(1, "Toby", "Goransson")),
                 new Book(3, "New Book", "Description of New Book", new Author(1, "Toby", "Goransson"))
                };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateBookCommand>(), default))
                .ReturnsAsync(booksListWhenUpdated);

            // Act
            var result = await _controller.CreateNewBook(newBook);

            // Assert
           
            var okResult = result as OkObjectResult; 
            Assert.IsNotNull(okResult); 
            Assert.AreEqual(200, okResult.StatusCode); 
            
            var books = okResult.Value as List<Book>;
            Assert.IsNotNull(books); 
            Assert.AreEqual(3, books.Count); 
            Assert.Contains(booksListWhenUpdated.Last(), books); 
        }
        [Test]
        public async Task CreateNewBook_ShouldReturnBadRequest_WhenBookIsNull()
        {
            // Arrange
            Book bookToAdd = null;

            // Act
            var result = await _controller.CreateNewBook(bookToAdd);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

      
        }
     


    }



}

