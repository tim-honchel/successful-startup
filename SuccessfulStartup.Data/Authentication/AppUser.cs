using Microsoft.AspNetCore.Identity;

namespace SuccessfulStartup.Data.Authentication
{
    public class AppUser : IdentityUser // class that represents the user, interface specific to Entity Framework
    {
        // contains virtual fields which can be overriden (Email, EmailConfirmed, Id, PasswordHash, TwoFactorEnabled, UserName, etc.)
    }
}
