using Microsoft.AspNetCore.Identity;

namespace Core.Entites;

public class ApplicationUser : IdentityUser
{
    public DateTime BirthDate { get; set; }

}