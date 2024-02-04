using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using LetsGame.Common.Data;

namespace Identity.API.Models.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IIdable<string>
    {

        [Required]
        [MaxLength(20)]
        [ProtectedPersonalData]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        [ProtectedPersonalData]
        public string LastName { get; set; }

        [MaxLength(80)]
        public string ProfilePicture { get; set; } = "/images/default-profile-pictures/default-profile-pic-1.jpg";

        // Metadata
        [Required]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now;

        // Friendships
        public virtual List<UserRelationship> RelationshipsAsRequester { get; set; } = new();
        public virtual List<UserRelationship> RelationshipsAsAddressee { get; set; } = new();

        public List<UserRelationship> Friends => RelationshipsAsRequester.Concat(RelationshipsAsAddressee).ToList();
    }
}
