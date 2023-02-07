using Microsoft.AspNetCore.Http; // for DefaultHttpContext
using Microsoft.AspNetCore.Identity; // for IdentityUser
using Microsoft.AspNetCore.Mvc; // for Controller, ControllerContext
using SuccessfulStartup.Api.Controllers;

namespace SuccessfulStartup.ApiTests
{
    internal class ApiHelper
    {
        public IdentityUser standardUser = new IdentityUser() // creates a standard user for all unit tests for predictable results
        {
            AccessFailedCount = 0,
            ConcurrencyStamp = "5f931d32-af66-4bfe-ad75-cc4f72d221e4",
            Email = "email@gmail.com",
            EmailConfirmed = true,
            Id = "00000000-0000-0000-0000-000000000000",
            LockoutEnabled = true,
            LockoutEnd = null,
            NormalizedEmail = "EMAIL@GMAIL.COM",
            NormalizedUserName = "EMAIL@GMAIL.COM",
            PasswordHash = "SDFASFDsdafsadfSFS24533251",
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            SecurityStamp = "799a234e-7cdd-4616-84c6-b983190a6d29",
            TwoFactorEnabled = false,
            UserName = "email@gmail.com"
        };

        public void AddRequestHeader(PlanController controller, bool valid = true)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["key"] = valid ? "valid-key" : "invalid-key";
            var controllerContext = new ControllerContext() { HttpContext = httpContext };
            controller.ControllerContext = controllerContext;
        }
    }
}
