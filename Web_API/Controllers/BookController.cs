using ApplicationBook.Authors.Commands.DeleteAuthor;
using ApplicationBook.Books.Commands.CreateBook;
using ApplicationBook.Books.Commands.DeleteBook;
using ApplicationBook.Books.Commands.UpdateBook;
using ApplicationBook.Books.Queries.GetBook;
using ApplicationBook.Books.Queries.GetBookById;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<BookController>
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _mediator.Send(new GetAllValuesQuery());
            return Ok(result);
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            // Validera att ID är ett positivt heltal
            if (id <= 0)
            {
                return BadRequest(new { Message = "The ID must be a positive integer." });
            }

            // Skicka förfrågan via Mediator
            var result = await _mediator.Send(new GetValueByIdQuery(id));

            // Kontrollera om boken hittades
            if (result == null)
            {
                return NotFound(new { Message = $"Book with ID {id} was not found." });
            }

            return Ok(result);
        }


        // POST api/<BookController>
        [HttpPost]
        public async Task<IActionResult> CreateNewBook([FromBody] Book bookToAdd)
        {
            // Kontrollera om den inkommande boken är null
            if (bookToAdd == null)
            {
                return BadRequest(new { Message = "The book data cannot be null." });
            }

            // Validera att titel och beskrivning är angivna
            if (string.IsNullOrWhiteSpace(bookToAdd.Title))
            {
                return BadRequest(new { Message = "The book title is required." });
            }

            if (string.IsNullOrWhiteSpace(bookToAdd.Description))
            {
                return BadRequest(new { Message = "The book description is required." });
            }

            // Validera att författare är angiven
            if (bookToAdd.Author == null)
            {
                return BadRequest(new { Message = "The book must have an author." });
            }

            // Skicka kommandot via Mediator
            var result = await _mediator.Send(new CreateBookCommand(bookToAdd));

            return Ok(result);
        }


        // PUT api/<BookController>/5

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                return BadRequest("ID:t i URL:en matchar inte ID:t i objektet.");
            }

            try
            {
                
                var result = await _mediator.Send(new UpdateBookCommand(updatedBook));

                return Ok(result); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new {Message = ex.Message});
            }
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var authorToRemove = new DeleteBookCommand(id);
            var result = await _mediator.Send(authorToRemove);
            if (result == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
