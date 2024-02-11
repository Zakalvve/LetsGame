using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using LetsGame.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Models.Data;

[Table("UserRelationships")]
[Index(nameof(RequesterId))]
[Index(nameof(AddresseeId))]
public class UserRelationship : IEntityMetaData
{
    [Required]
    public string RequesterId { get; set; }
    public virtual ApplicationUser Requester { get; set; } = null!;

    [Required]
    public string AddresseeId { get; set; }
    public virtual ApplicationUser Addressee { get; set; } = null!;

    public bool PendingAccept { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime LastModified { get; set; }
}
