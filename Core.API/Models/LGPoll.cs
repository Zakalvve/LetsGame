using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.API.Models
{
    [Table("LGPolls")]
    [PrimaryKey("Id")]
    public class LGPoll
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
