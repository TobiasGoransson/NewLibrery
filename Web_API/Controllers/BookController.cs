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
            var result = await _mediator.Send(new GetAllValuesQuery());
            _logger.LogInformation("Fetched {Count} books successfully.", result.Count);
            return Ok(result);
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Fetching book with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided: {Id}", id);
                return BadRequest(new { Message = "The ID must be a positive integer." });
            }

            var result = await _mediator.Send(new GetValueByIdQuery(id));

            if (result == null)
            {
                _logger.LogWarning("Book with ID {Id} was not found.", id);
                return NotFound(new { Message = $"Book with ID {id} was not found." });
            }

            _logger.LogInformation("Book with ID {Id} fetched successfully.", id);
            return Ok(result);
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

            var result = await _mediator.Send(new CreateBookCommand(bookToAdd));

            _logger.LogInformation("Book created successfully with ID: {Id}", result);
            return Ok(result);
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            _logger.LogInformation("Updating book with ID: {Id}", id);

            if (id != updatedBook.Id)
            {
                _logger.LogWarning("ID mismatch: URL ID {UrlId} does not match body ID {BodyId}.", id, updatedBook.Id);
                return BadRequest("ID:t i URL:en matchar inte ID:t i objektet.");
            }

            try
            {
                var result = await _mediator.Send(new UpdateBookCommand(updatedBook));
                _logger.LogInformation("Book with ID {Id} updated successfully.", id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Update failed: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            _logger.LogInformation("Deleting book with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided: {Id}", id);
                return BadRequest("Invalid ID. ID must be a positive integer.");
            }

            var command = new DeleteBookCommand(id);
            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogWarning("Book with ID {Id} was not found for deletion.", id);
                return NotFound(new { Message = $"Book with ID {id} was not found." });
            }

            _logger.LogInformation("Book with ID {Id} deleted successfully.", id);
            return NoContent();
        }
    }
}
