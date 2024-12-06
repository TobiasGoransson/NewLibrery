using ApplicationBook.Users.Commands.RegisterNewUser;
using ApplicationBook.Users.Dtos;
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
            _logger.LogInformation("Fetching all users...");

            try
            {
                var users = await _mediator.Send(new GetAllUsersQuery());
                _logger.LogInformation("Successfully fetched {Count} users.", users.Count);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
                return StatusCode(500, new { Message = "An error occurred while fetching users." });
            }
        }

        // POST: api/<UserController>/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterNewUserCommand command)
        {
            _logger.LogInformation("Registering a new user with username: {Username}", command.NewUser);

            try
            {
                if (command == null)
                {
                    _logger.LogWarning("Register user request body was null.");
                    return BadRequest(new { Message = "The request body cannot be null." });
                }

                var newUser = await _mediator.Send(command);
                _logger.LogInformation("User with username {Username} registered successfully.", newUser.UserName);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering a new user.");
                return StatusCode(500, new { Message = "An error occurred while registering the user." });
            }
        }

        // POST: api/<UserController>/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LogIn([FromBody] LogInUserQuery query)
        {
            _logger.LogInformation("User attempting to log in with username: {Username}", query.LogInUser);

            try
            {
                if (query == null)
                {
                    _logger.LogWarning("Login request body was null.");
                    return BadRequest(new { Message = "The request body cannot be null." });
                }

                var token = await _mediator.Send(query);

                if (token == null)
                {
                    _logger.LogWarning("Login failed for username: {Username}", query.LogInUser);
                    return Unauthorized(new { Message = "Invalid username or password." });
                }

                _logger.LogInformation("User {Username} logged in successfully.", query.LogInUser);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for username: {Username}", query.LogInUser);
                return StatusCode(500, new { Message = "An error occurred while logging in." });
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

