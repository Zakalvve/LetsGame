using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LetsGame.Common.Data.Entities.Core;
namespace Core.API.Models;


[Table("LGEvents")]
public class LGEvent : LetsGameBaseEntity
{
    public string Name { get; set; }
}
