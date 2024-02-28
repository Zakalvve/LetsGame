using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using LetsGame.Common.Data.Entities.Core;

namespace Identity.API.Models.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IIdable<string>
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string ProfilePicture { get; set; }

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
