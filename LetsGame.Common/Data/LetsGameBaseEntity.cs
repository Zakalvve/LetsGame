
using System.ComponentModel.DataAnnotations;

namespace LetsGame.Common.Data;

public class LetsGameBaseEntity : IIdable<int>
{
    [Key]
    public virtual int Id { get; set; }
    [Required]
    public virtual DateTime DateAdded { get; set; }
    [Required]
    public virtual DateTime LastModified { get; set; }
}
