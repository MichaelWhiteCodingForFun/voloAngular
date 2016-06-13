using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Entities
{
    public class TicketTable
    {
        public string TableName { get; set; }
        public List<TicketDocument> Rows { get; set; }
    }
}
