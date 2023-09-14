using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EmailSender.Data.Entities.Users
{
    public class AppUser : IdentityUser
    {
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
