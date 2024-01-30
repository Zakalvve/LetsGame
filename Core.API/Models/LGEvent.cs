using System.ComponentModel.DataAnnotations;

namespace Core.API.Models;

public class LGEvent
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}
