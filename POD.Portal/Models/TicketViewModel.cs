using POD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POD.Portal.Models
{
    public class TicketViewModel
    {
        public Guid ID { get; set; }
        public Guid OrganizationID { get; set; }
        public string SerialNumber { get; set; }
        public int TicketNumber { get; set; }
        public string Plant { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string HaulierCode { get; set; }
        public string HaulierReg { get; set; }
        public string QuoteNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string PayerAccount { get; set; }
        public string DeletionInd { get; set; }

        public static implicit operator TicketViewModel(Ticket ticketEntity)
        {
            return new TicketViewModel
            {
                ID = ticketEntity.ID,
                TicketNumber = ticketEntity.TicketNumber,
                AccountNumber = ticketEntity.AccountNumber,
                AccountName = ticketEntity.AccountName,
                DeliveryDate = ticketEntity.DeliveryDate,
                InvoiceNumber = ticketEntity.InvoiceNumber,
                PayerAccount = ticketEntity.PayerAccount
            };
        }

        public static implicit operator Ticket(TicketViewModel ticketViewModel)
        {
            return new Ticket
            {
                ID = ticketViewModel.ID,
                TicketNumber = ticketViewModel.TicketNumber,
                AccountNumber = ticketViewModel.AccountNumber,
                AccountName = ticketViewModel.AccountName,
                DeliveryDate = ticketViewModel.DeliveryDate,
                InvoiceNumber = ticketViewModel.InvoiceNumber,
                PayerAccount = ticketViewModel.PayerAccount
            };
        }

    }
    public class TicketsViewModel : BaseViewModel
    {
        public List<TicketViewModel> Tickets { get; set; }

        public Guid? OrganizationID { get; set; }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int? TicketNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}