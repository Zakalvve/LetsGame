using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using LetsGame.Common.Data.Entities.Core;

namespace Core.API.Models
{
    [Table("LGPolls")]
    public class LGPoll : LetsGameBaseEntity
    {
        public string Title { get; set; }
    }
}
