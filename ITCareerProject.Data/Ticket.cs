using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCareerProject.Data
{
    public class Ticket
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
