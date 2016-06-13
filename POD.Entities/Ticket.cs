using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace POD.Entities
{
    public class Ticket
    {
        public Guid ID { get; set; }
        public Guid OrganizationID { get; set; }
        public string SerialNumber { get; set; }
        public int TicketNumber { get; set; }
        public string Plant { get; set; }
        public DateTime? DeliveryDate { get; set; }       
        public string HaulierCode { get; set; }
        public string HaulierReg { get; set; }
        public string QuoteNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string PayerAccount { get; set; }
        public string DeletionInd { get; set; }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
    }

    public class TicketMapper : ClassMapper<Ticket>
    {
        public TicketMapper()
        {
            base.Map(u => u.AccountNumber).Ignore();
            base.Map(u => u.AccountName).Ignore();
            base.AutoMap();
        }
    }
}
