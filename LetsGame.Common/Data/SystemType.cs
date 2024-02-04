
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LetsGame.Common.Data;

[Table("SystemTypes")]
public class SystemType : LetsGameBaseEntity
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(30)]
    public string? Description { get; set; }
    public virtual ICollection<SystemTypeValue> SystemTypeValues { get; } = new List<SystemTypeValue>();
}

[Table("SystemTypeValues")]
public class SystemTypeValue : LetsGameBaseEntity
{
    [Required]
    public int SystemTypeId { get; set; }
    public virtual SystemType SystemType { get; set; } = null!;

    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(30)]
    public string? Description { get; set; }
}
