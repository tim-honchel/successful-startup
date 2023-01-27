﻿using Microsoft.AspNetCore.Mvc; // for ControllerBase, HttpGet
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase // API endpoint for HTTP requests
    {
        private IUserReadOnlyRepository _repositoryForReadingUsers;

        public UserController(IUserReadOnlyRepository repositoryForReadingUsers)
        {
            _repositoryForReadingUsers = repositoryForReadingUsers;
        }

        [HttpGet("{username}")]
        public virtual async Task<IActionResult> GetUserIdByUsername(string username)
        {
            var id = await _repositoryForReadingUsers.GetUserIdByUsernameAsync(username);
            return new OkObjectResult(id);
        }
    }
}
