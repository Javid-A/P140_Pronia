using Microsoft.AspNetCore.Identity;

namespace P140_Pronia.Entities
{
    public class CustomUser:IdentityUser
    {
        public string Fullname { get; set; } = null!;
    }
}
