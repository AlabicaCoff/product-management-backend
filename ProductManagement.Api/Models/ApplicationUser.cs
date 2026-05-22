using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Api.Models
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public required string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public required string LastName { get; set; }

        [PersonalData]
        public DateTime CreatedDate { get; set; }
    }
}
