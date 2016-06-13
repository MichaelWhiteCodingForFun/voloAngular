using POD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Interfaces
{
   public  interface ITicketDataService
    {
        Guid InsertTicket(Ticket ticket);
        bool UpdateTicket(Ticket ticket);
        //List<Ticket> ConvertFromExcel(string filePath);
        //Guid IsOrganizationExists(int accountNumber);
        //Guid CreateOrganization(Ticket ticket);
        Ticket GetTicket(int ticketNumber);
        IEnumerable<Ticket> GetTickets(Guid organizationID, string accontNumber, string accountName, int? ticketNumber, DateTime? deliveryDate, string invoiceNumber, int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows);
        List<TicketTable> SearchDocument(string accountNumber = null, int? ticketNumber = null, DateTime? deliveryDate = null);
        byte[] RetrieveDocument(int documentID);
    }
}
