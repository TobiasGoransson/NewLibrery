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
using ApplicationBook.Dtos;


[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthorController> _logger;
    public AuthorController(IMediator mediator, ILogger<AuthorController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    // GET: api/author
    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAuthors()
    {
        _logger.LogInformation("Fetching all authors from the database.");
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
            _logger.LogError(ex, "An error occurred while fetching authors.");
        }
    }

    // GET: api/author/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthorById(int id)
    {
        var query = new GetAuthorByIdQuery(id);

        _logger.LogInformation($"Fetching author with ID {id} from the database.");

        var author = await _mediator.Send(query);
        if (author == null)
        {
            return NotFound(new { Message = $"Author with ID {id} was not found." });

            _logger.LogWarning($"Author with ID {id} was not found.");
        }
        return Ok(author);
    }



    // POST: api/author
    [HttpPost]
    [Route("CreateAuthor")]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorCommand command)
    {
        _logger.LogInformation("Creating a new author.");
        try
        { 
        // Validera att förnamn och efternamn inte är tomma
            if (command == null)
            {
            return BadRequest("Both FirstName and LastName must be provided.");
            _logger.LogWarning("Both FirstName and LastName must be provided.");
             }

            var createdAuthor = await _mediator.Send(command);
            return Ok(createdAuthor);
        }
        catch
        {
        // Om inget objekt skapas, returnera ett internt fel
      
            return StatusCode(500, "An error occurred while creating the author.");
            _logger.LogError("An error occurred while creating the author.");
        }

        
    }


    // PUT: api/author/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorCommand command)
    {
        _logger.LogInformation("UpdateAuthor called with ID: {Id} and Command: {@Command}", id, command);

        if (command == null)
        {
            _logger.LogWarning("UpdateAuthor request body is null.");
            return BadRequest("The request body cannot be null.");
        }

        if (id != command.Id)
        {
            _logger.LogWarning("UpdateAuthor ID mismatch: URL ID {UrlId} does not match Command ID {CommandId}.", id, command.Id);
            return BadRequest("The provided ID does not match the ID in the request body.");
        }

        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
        {
            _logger.LogWarning("UpdateAuthor validation failed: FirstName or LastName is missing.");
            return BadRequest("Both FirstName and LastName are required.");
        }

        try
        {
            var updatedAuthor = await _mediator.Send(command);
            if (updatedAuthor == null)
            {
                _logger.LogWarning("UpdateAuthor: Author with ID {Id} not found.", id);
                return NotFound(new { Message = $"Author with ID {id} was not found." });
            }

            _logger.LogInformation("UpdateAuthor succeeded for ID: {Id}.", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating Author with ID: {Id}.", id);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    // DELETE: api/author/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        _logger.LogInformation("DeleteAuthor called with ID: {Id}.", id);

        if (id <= 0)
        {
            _logger.LogWarning("DeleteAuthor failed validation: Invalid ID {Id}.", id);
            return BadRequest("Invalid ID. ID must be a positive integer.");
        }

        var authorToRemove = new DeleteAuthorCommand(id);

        try
        {
            var result = await _mediator.Send(authorToRemove);
            if (result == null)
            {
                _logger.LogWarning("DeleteAuthor: Author with ID {Id} not found.", id);
                return NotFound(new { Message = $"Author with ID {id} was not found." });
            }

            _logger.LogInformation("DeleteAuthor succeeded for ID: {Id}.", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting Author with ID: {Id}.", id);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }


}