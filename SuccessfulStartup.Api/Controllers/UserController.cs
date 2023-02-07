using Microsoft.AspNetCore.Mvc; // for ControllerBase
using Microsoft.EntityFrameworkCore; // for DbUpdateException
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserWriteOnlyRepository _repositoryForWritingUsers;

        public UserController(IUserWriteOnlyRepository repositoryForWritingUsers)
        {
            _repositoryForWritingUsers = repositoryForWritingUsers;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Dictionary<string, string> user)
        {
            try
            {
                _repositoryForWritingUsers.AddUserAsync(user["userId"], user["securityStamp"]);
                return new OkResult();
            }
            catch (ArgumentNullException)
            {
                return new BadRequestResult();
            }
            catch (InvalidOperationException)
            {
                return new UnauthorizedResult();
            }
            catch (DbUpdateException)
            {
                return new NoContentResult();
            }

        }


    }
}
