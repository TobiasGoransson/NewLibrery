using ApplicationBook.Authors.Commands.DeleteAuthor;
using ApplicationBook.Books.Commands.CreateBook;
using ApplicationBook.Books.Commands.DeleteBook;
using ApplicationBook.Books.Commands.UpdateBook;
using ApplicationBook.Books.Queries.GetBook;
using ApplicationBook.Books.Queries.GetBookById;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookController> _logger;

        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/<BookController>
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            _logger.LogInformation("Fetching all books...");

            var operationResult = await _mediator.Send(new GetAllValuesQuery());

            if (!operationResult.Success)
            {
                _logger.LogWarning("Failed to fetch books. Error: {Error}", operationResult.ErrorMessage);
                return BadRequest(new { Message = operationResult.ErrorMessage });
            }

            _logger.LogInformation("Fetched {Count} books successfully.", operationResult.Data.Count);
            return Ok(new { Message = operationResult.Message, Data = operationResult.Data });
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            try
            {
                var operationResult = _mediator.Send(new GetValueByIdQuery(id)).Result;
                _logger.LogInformation("Fetching book with ID: {Id}", id);

                if (operationResult.Success)
                {
                    _logger.LogInformation("Book with ID {Id} fetched successfully.", id);
                    return Ok(new { message = operationResult.Message, data = operationResult.Data });
                }
                else
                {
                    _logger.LogWarning("Invalid ID provided: {Id}", id);
                    return BadRequest(new { message = operationResult.Message, errors = operationResult.ErrorMessage });

                }
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });

            }   

           
        }

        // POST api/<BookController>
        [HttpPost]
        public async Task<IActionResult> CreateNewBook([FromBody] Book bookToAdd)
        {
            _logger.LogInformation("Creating a new book...");

            if (bookToAdd == null)
            {
                _logger.LogWarning("Book data was null.");
                return BadRequest(new { Message = "The book data cannot be null." });
            }

            if (string.IsNullOrWhiteSpace(bookToAdd.Title) || string.IsNullOrWhiteSpace(bookToAdd.Description))
            {
                _logger.LogWarning("Invalid book data: Missing title or description.");
                return BadRequest(new { Message = "The book title and description are required." });
            }

            if (bookToAdd.Author == null)
            {
                _logger.LogWarning("Invalid book data: Missing author.");
                return BadRequest(new { Message = "The book must have an author." });
            }

            var operationResult = await _mediator.Send(new CreateBookCommand(bookToAdd));

            if (!operationResult.Success)
            {
                _logger.LogWarning("Failed to create book. Error: {Error}", operationResult.ErrorMessage);
                return BadRequest(new { Message = operationResult.ErrorMessage });
            }

            _logger.LogInformation("Book created successfully.");
            return Ok(new { Message = operationResult.Message, Data = operationResult.Data });
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            _logger.LogInformation("Updating book with ID: {Id}", id);

            if (id != updatedBook.BId)
            {
                _logger.LogWarning("ID mismatch: URL ID {UrlId} does not match body ID {BodyId}.", id, updatedBook.BId);
                return BadRequest(new { Message = "The ID in the URL does not match the ID in the provided book." });
            }

            var operationResult = await _mediator.Send(new UpdateBookCommand(updatedBook));

            if (!operationResult.Success)
            {
                _logger.LogWarning("Failed to update book with ID: {Id}. Error: {Error}", id, operationResult.ErrorMessage);
                return BadRequest(new { Message = operationResult.ErrorMessage });
            }

            _logger.LogInformation("Book with ID {Id} updated successfully.", id);
            return Ok(new { Message = operationResult.Message, Data = operationResult.Data });
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            _logger.LogInformation("Deleting book with ID: {Id}", id);

            var operationResult = await _mediator.Send(new DeleteBookCommand(id));

            if (!operationResult.Success)
            {
                _logger.LogWarning("Failed to delete book with ID: {Id}. Error: {Error}", id, operationResult.ErrorMessage);
                return BadRequest(new { Message = operationResult.ErrorMessage });
            }

            _logger.LogInformation("Book with ID {Id} deleted successfully.", id);
            return Ok(new { Message = operationResult.Message, Data = operationResult.Data });
        }
    }
}
