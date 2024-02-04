using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LetsGame.Common.Data;

namespace Identity.API.Models.Data;

[Table("UserRelationships")]
public class UserRelationship : IEntityMetaData
{
    [Required]
    public string RequesterId { get; set; }
    public virtual ApplicationUser Requester { get; set; } = null!;

    [Required]
    public string AddresseeId { get; set; }
    public virtual ApplicationUser Addressee { get; set; } = null!;

    public bool PendingAccept { get; set; } = true;
    public DateTime DateAdded { get; set; }
    public DateTime LastModified { get; set; }
}
