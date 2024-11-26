using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain;
using ApplicationBook.Authors.Commands.CreateAuthor;
using ApplicationBook.Authors.Commands.UpdateAuthor;
using ApplicationBook.Authors.Commands.DeleteAuthor;
using ApplicationBook.Authors.Queries.GetAllAuthors;
using ApplicationBook.Authors.Queries.GetAuthorById;


[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/author
    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAuthors()
    {
        try
        {
            var query = new GetAllAuthorsQuery();
            var authors = await _mediator.Send(query);

            // Validering: Kontrollera om listan är tom
            if (authors == null || authors.Count == 0)
            {
                return NoContent(); // Returnerar 204 om inga författare hittas
            }

            return Ok(authors);
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, new { Message = "An error occurred while fetching authors.", Details = ex.Message });
        }
    }

    // GET: api/author/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthorById(int id)
    {
        var query = new GetAuthorByIdQuery(id);
        var author = await _mediator.Send(query);
        if (author == null)
        {
            return NotFound(new { Message = $"Author with ID {id} was not found." });
        }
        return Ok(author);
    }



    // POST: api/author
    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor([FromBody] CreateAuthorCommand command)
    {
        // Validera att kommandot inte är null
        if (command == null)
        {
            return BadRequest("The request body cannot be null.");
        }

        // Validera att förnamn och efternamn inte är tomma
        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
        {
            return BadRequest("Both FirstName and LastName must be provided.");
        }

        var createdAuthor = await _mediator.Send(command);

        // Om inget objekt skapas, returnera ett internt fel
        if (createdAuthor == null)
        {
            return StatusCode(500, "An error occurred while creating the author.");
        }

        return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.Id }, createdAuthor);
    }


    // PUT: api/author/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorCommand command)
    {
        // Validera att kommandot inte är null
        if (command == null)
        {
            return BadRequest("The request body cannot be null.");
        }

        // Validera att id i URL matchar id i kommandot
        if (id != command.Id)
        {
            return BadRequest("The provided ID does not match the ID in the request body.");
        }

        // Validera att både FirstName och LastName inte är tomma
        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
        {
            return BadRequest("Both FirstName and LastName are required.");
        }

        // Skicka kommandot för att uppdatera författaren
        var updatedAuthor = await _mediator.Send(command);
        if (updatedAuthor == null)
        {
            return NotFound(new { Message = $"Author with ID {id} was not found." });
        }

        return NoContent();
    }



    // DELETE: api/author/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        // Validera att id är större än 0
        if (id <= 0)
        {
            return BadRequest("Invalid ID. ID must be a positive integer.");
        }

        // Skapa DeleteAuthorCommand
        var authorToRemove = new DeleteAuthorCommand(id);

        // Skicka kommandot till hanteraren
        var result = await _mediator.Send(authorToRemove);

        // Om resultatet är null betyder det att författaren inte hittades
        if (result == null)
        {
            return NotFound(new { Message = $"Author with ID {id} was not found." });
        }

        // Om författaren tas bort, returnera NoContent (HTTP 204)
        return NoContent();
    }

}