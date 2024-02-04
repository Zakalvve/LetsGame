using LetsGame.Common.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Identity.API.Models.Data
{
    
    [PrimaryKey("Id")]
    [Table("ProfilePictures")]
    public class ProfilePicture : LetsGameBaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string imagePath { get; set; }
    }
}
