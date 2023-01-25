using Microsoft.AspNetCore.Mvc;
using SuccessfulStartup.Data.Repositories.ReadOnly;

namespace SuccessfulStartup.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase // API endpoint for HTTP requests
    {
        private UserReadOnlyRepository _repositoryForReadingUsers;

        public UserController(UserReadOnlyRepository repositoryForReadingUsers)
        {
            _repositoryForReadingUsers = repositoryForReadingUsers;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserIdByUsername(string username)
        {
            var id = await _repositoryForReadingUsers.GetUserIdByUsernameAsync(username);
            return new OkObjectResult(id);
        }
    }
}
