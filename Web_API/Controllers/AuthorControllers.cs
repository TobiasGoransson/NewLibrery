




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
using Domain.Dtos;


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
    [Route("GetAllAuthors")]
    public async Task<IActionResult> GetAllAuthors()
    {
        _logger.LogInformation("GetAllAuthors endpoint called.");

        var result = await _mediator.Send(new GetAllAuthorsQuery());

        if (result.Success)
        {
            _logger.LogInformation("Successfully retrieved authors.");
            return Ok(result.Data); // Returnera listan över författare
        }
        else
        {
            _logger.LogWarning("Failed to retrieve authors: {ErrorMessage}", result.ErrorMessage);
            return new NotFoundObjectResult(new { Message = "No authors found." });

        }
    }


    // GET: api/author/{id}
    [HttpGet]
    [Route("GetAuthorById/{id}")]
    public async Task<IActionResult> GetAuthorById(int id)
    {
        _logger.LogInformation("GetAuthorById endpoint called with ID {AuthorId}.", id);

        var result = await _mediator.Send(new GetAuthorByIdQuery (id));

        if (result.Success)
        {
            _logger.LogInformation("Author found: {Author}", result.Data);
            return Ok(result.Data);
        }
        else
        {
            _logger.LogWarning("Author not found: {ErrorMessage}", result.ErrorMessage);
            return NotFound(result.ErrorMessage); // Return NotFoundObjectResult
        }
    }





    // POST: api/author
    [HttpPost]
    [Route("CreateAuthor")]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorCommand command)
    {
        _logger.LogInformation("Creating a new author.");

        if (command == null || string.IsNullOrEmpty(command.NewAuthor.FirstName) || string.IsNullOrEmpty(command.NewAuthor.LastName))
        {
            _logger.LogWarning("Both FirstName and LastName must be provided.");
            return BadRequest("Both FirstName and LastName must be provided.");
        }

        var result = await _mediator.Send(command);

        if (result.Success)
        {
            return Ok(result.Data); // Return created author if successful
        }
        else
        {
            _logger.LogError(result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage); // Return error message if failed
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

        var result = await _mediator.Send(command);

        if (result.Success)
        {
            _logger.LogInformation("UpdateAuthor succeeded for ID: {Id}.", id);
            return NoContent(); // Lyckad uppdatering
        }
        else
        {
            if (result.ErrorMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("UpdateAuthor: Author with ID {Id} not found.", id);
                return NotFound(new { Message = result.ErrorMessage }); // Returnera 404 om författaren inte hittades
            }

            _logger.LogError("UpdateAuthor failed for ID: {Id}. Error: {ErrorMessage}", id, result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage); // Returnera 500 för oväntade fel
        }
    }


    // DELETE: api/author/{id}
    [HttpDelete]
    [Route("DeleteAuthor/{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        _logger.LogInformation("Deleting author with ID {AuthorId}.", id);

        var command = new DeleteAuthorCommand(id);
        var result = await _mediator.Send(command);

        if (result.Success)
        {
            _logger.LogInformation("Successfully deleted author with ID {AuthorId}.", id);
            return Ok(result.Data); // Returnera den borttagna författaren
        }
        else
        {
            _logger.LogWarning(result.ErrorMessage);
            return NotFound(result.ErrorMessage); // Returnera 404 om författaren inte hittades
        }
    }



}