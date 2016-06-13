using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Portal.Models
{
    public class ReportViewModel
    {
        public int ID {get; set; }
        public int ReportTypeID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string Details { get; set; }
        public int TicketID { get; set; }
        public int CreatedByID { get; set; }
    }
}
