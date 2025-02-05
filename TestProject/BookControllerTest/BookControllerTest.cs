using ApplicationBook.Books.Commands.CreateBook;
using ApplicationBook.Books.Commands.DeleteBook;
using ApplicationBook.Books.Commands.UpdateBook;
using ApplicationBook.Books.Queries.GetBook;
using ApplicationBook.Books.Queries.GetBookById;
using Domain;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Web_API.Controllers;

namespace BookControllerTest
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<BookController>> _loggerMock;
        private BookController _controller;
        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<BookController>>();
            _controller = new BookController(_mediatorMock.Object, _loggerMock.Object);
        }
        [Test]
        public async Task Get_ShouldReturnOkResult_WithListOfBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book (1,"BookTest","TestOfBook" ),
                new Book (2, "BookTest2","TestOfBook2")
            };
            var operationResult = OperationResult<List<Book>>.Successfull(books);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetBooksQuery>(), default))
                .ReturnsAsync(operationResult);
            // Act
            var result = await _controller.GetBooks();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

        }
        [Test]
        public async Task GetById_ShouldReturnOkResult_WithSingleBook()
        {
            // Arrange
            var book = new Book(1, "BookTest2", "TestOfBook2");
            var operationResult = OperationResult<Book>.Successfull(book);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetBookByIdQuery>(), default))
                .ReturnsAsync(operationResult);
            // Act
            var result = await _controller.GetBookById(1);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

        }
     

 

        [Test]
        public async Task CreateNewBook_ShouldAddBook_WhenValidBookIsProvided_And_Return_List_Including_NewBook()
        {
            // Arrange
            var newBook = new BookDto ( "New Book", "Toby Goransson", 1 , "Jane", "Austin" );

            var booksListWhenUpdated = new List<BookDto>
            {
                new BookDto ("TobyBook", "Toby Goransson" , 1 , "Jane", "Austin"),
                new BookDto ("TobyBook2", "Toby Goransson", 1 , "Jane", "Austin"),
                new BookDto ( "New Book", "Toby Goransson" , 1 , "Jane", "Austin")
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateBookCommand>(), default))
                .ReturnsAsync(OperationResult<BookDto>.Successfull(newBook));

            // Act
            var result = await _controller.CreateNewBook(newBook);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

           
        }

        [Test]
        public async Task UpdateBook_ShouldReturnOk_WhenBookIsUpdated()
        {
            // Arrange
            var bookIdToUpdate = 1;
            var updatedBook = new Book (1, "Updated Title",  "Updated Author");

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), default))
                .ReturnsAsync(OperationResult<Book>.Successfull(updatedBook));

            // Act
            var result = await _controller.UpdateBook(bookIdToUpdate, updatedBook);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            
        }
        [Test]
        public async Task DeleteBook_ValidId_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var bookId = 5;  // Specifiera ID för boken som ska tas bort
            var operationResult = OperationResult<List<Book>>.Successfull(new List<Book>(), "Book deleted successfully");

            // Setup: Mocka Mediator-anropet
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteBookCommand>(), default))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.DeleteBook(bookId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result); // Kontrollera att resultatet är OkObjectResult
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult); // Kontrollera att okResult inte är null
            Assert.AreEqual(200, okResult.StatusCode); // Kontrollera HTTP-statuskod är 200


        }

       

        [Test]
        public async Task CreateNewBook_ShouldReturnBadRequest_WhenBookIsNull()
        {
            // Arrange
            BookDto bookToAdd = null;

            // Act
            var result = await _controller.CreateNewBook(bookToAdd);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }
    }
}