using Microsoft.AspNetCore.Mvc; // for ControllerBase
using Microsoft.EntityFrameworkCore; // for DbUpdateException
using SuccessfulStartup.Domain.Repositories.WriteOnly;
using Swashbuckle.AspNetCore.Annotations; // for SwaggerOperation, ProducesResponseType

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
        [SwaggerOperation(Summary = "Add a user", Description="Include the User ID and API key")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        public async Task<IActionResult> AddUser(Dictionary<string, string> user)
        {
            try
            {
                await _repositoryForWritingUsers.AddUserAsync(user["userId"], user["securityStamp"]);
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
