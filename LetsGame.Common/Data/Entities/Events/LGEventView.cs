using System.ComponentModel.DataAnnotations;
using LetsGame.Common.Data.DataAnnotations;
using LetsGame.Common.Data.Entities.Users;

namespace LetsGame.Common.Data.Entities.Events
{
    public class LGEventView
    {
        //[Required]
        //[MaxLength(255, ErrorMessage = "Title must be less than 256 characters.")]
        public string Title { get; set; }

        //[MaxLength(3000, ErrorMessage = "Description must be less than 3000 characters.")]
        public string? Description { get; set; }

        //[BeforeEndDate(EndDatePropertyName = nameof(EndDate), AllowEqualDates = true, ErrorMessage = "Event start date must occur before event end date.")]
        //[FutureDate(ErrorMessage = "Start date must be in the future")]
        public DateTime? StartDate { get; set; }

        //[AfterStartDate(StartDatePropertyName = nameof(StartDate), AllowEqualDates = true, ErrorMessage = "Event end date must occur before event start date.")]
        //[FutureDate(ErrorMessage = "End date must be in the future")]
        public DateTime? EndDate { get; set; }

        public string? Location { get; set; }
        public List<LGUserView> Attendees { get; set; } = new List<LGUserView>();
    }
}
