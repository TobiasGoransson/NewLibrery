using ApplicationBook.Users.Commands.RegisterNewUser;
using ApplicationBook.Dtos;
using ApplicationBook.Users.Queries.GetAllUsers;
using ApplicationBook.Users.Queries.LogIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/<UserController>/GetAllUsers
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers endpoint called.");

            var result = await _mediator.Send(new GetAllUsersQuery());

            if (result.Success)
            {
                _logger.LogInformation("Successfully retrieved {UserCount} users.", result.Data.Count);
                return Ok(result.Data); // Returnera listan med användare
            }
            else
            {
                _logger.LogWarning("Failed to retrieve users. Error: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(new { Message = result.ErrorMessage }); // Returnera felmeddelandet
            }
        }


        // POST: api/<UserController>/Register
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterNewUserCommand command)
        {
            _logger.LogInformation("RegisterUser endpoint called for UserName: {UserName}", command.NewUser.UserName);

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                _logger.LogInformation("User successfully registered with UserName: {UserName}", result.Data.UserName);
                return Ok(result.Data); // Returnera den registrerade användaren
            }
            else
            {
                _logger.LogWarning("User registration failed: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(new { Message = result.ErrorMessage }); // Returnera felmeddelandet
            }
        }


        // POST: api/<UserController>/Login
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] UserDto logInUser)
        {
            _logger.LogInformation("LogIn endpoint called for UserName: {UserName}", logInUser.UserName);

            var result = await _mediator.Send(new LogInUserQuery(logInUser));

            if (result.Success)
            {
                _logger.LogInformation("LogIn succeeded for UserName: {UserName}", logInUser.UserName);
                return Ok(new { Token = result.Data });
            }
            else
            {
                _logger.LogWarning("LogIn failed for UserName: {UserName}. Error: {ErrorMessage}", logInUser.UserName, result.ErrorMessage);
                return BadRequest(new { Message = result.ErrorMessage });
            }
        }

    }
}


        //[HttpPost]
        //[Route("RegisterUser")]
        //public async Task<IActionResult> RegisterUser([FromBody] UserDto NewUser)
        //{
        //    return Ok(await _mediator.Send(new RegisterNewUserCommand ( NewUser)));
        //}

        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> Login([FromBody] UserDto UserWaitingToLogIn)
        //{
        //    return Ok(await _mediator.Send(new LogInUserQuery(UserWaitingToLogIn)));
        //}
        //// GET api/<UserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<UserController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

