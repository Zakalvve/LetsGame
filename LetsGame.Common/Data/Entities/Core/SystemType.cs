using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LetsGame.Common.Data.Entities.Core;

[Table("SystemTypes")]
public class SystemType : LetsGameBaseEntity
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(120)]
    public string? Description { get; set; }
    public virtual ICollection<SystemTypeValue> SystemTypeValues { get; } = new List<SystemTypeValue>();
}
