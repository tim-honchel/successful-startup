using Microsoft.AspNetCore.Mvc; // for ControllerBase, HttpGet
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase // API controller with endpoints for HTTP requests
    {
        private readonly IUserReadOnlyRepository _repositoryForReadingUsers;

        public UserController(IUserReadOnlyRepository repositoryForReadingUsers)
        {
            _repositoryForReadingUsers = repositoryForReadingUsers;
        }

        [HttpGet("{username}")]
        public virtual async Task<IActionResult> GetUserIdByUsername(string username)
        {
            try
            {
                var id = await _repositoryForReadingUsers.GetUserIdByUsernameAsync(username);
                return new OkObjectResult(id);
            }
            catch (ArgumentNullException)
            {
                return new BadRequestObjectResult("null");
            }
            catch (NullReferenceException)
            {
                return new NotFoundObjectResult("not found");
            }


        }
    }
}
