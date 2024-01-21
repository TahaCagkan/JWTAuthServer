using Microsoft.AspNetCore.Identity;

namespace JWTAuthServer.Core.Entity
{
    public class UserApp:IdentityUser
    {
        public string City { get; set; }
    }
}
