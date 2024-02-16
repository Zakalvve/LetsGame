using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsGame.Common.Entities.Events
{
    public class LGEventView
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string Location { get; set; }
        public int Attendees { get; set; }
    }
}
