using Ninject;
using POD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using POD.Portal.Models;
using POD.Entities;
using POD.Portal.Helpers;
using System.IO;

namespace POD.Portal.Controllers.WebApi
{
    [RoutePrefix("api/tickets")]
    public class TicketController : ApiController
    {
        [Inject]
        public ITicketDataService _ticketDataService { get; set; }
        public TicketController(ITicketDataService ticketDataService)
        {
            _ticketDataService = ticketDataService;
        }


        [Authorize(Roles = "System Admin, Company Admin, Customer Admin")]
        [Route("GetTickets")]
        [HttpPost]
        public async Task<IHttpActionResult> GetTickets(HttpRequestMessage request, [FromBody]  TicketsViewModel ticketsViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                TicketsViewModel result = new TicketsViewModel();
                UserIdentityInfo userinfo = UserHelper.GetUserInfo(this);
                if (userinfo != null)
                {
                    int currentPageNumber = ticketsViewModel.CurrentPageNumber;
                    int pageSize = ticketsViewModel.PageSize;
                    string sortExpression = ticketsViewModel.SortExpression;
                    string sortDirection = ticketsViewModel.SortDirection;
                    int totalRows;

                    //if the OrganizationID comes with the request, than it is a Company admin in Customer view, else search with the current logged in OrganizationID
                    Guid organizationID = (ticketsViewModel.OrganizationID == null || ticketsViewModel.OrganizationID == default(Guid)) ? userinfo.OrganizationID : ticketsViewModel.OrganizationID.Value;

                    IEnumerable<Ticket> ticketlist = _ticketDataService.GetTickets(organizationID, ticketsViewModel.AccountNumber, ticketsViewModel.AccountName, 
                                                                                    ticketsViewModel.TicketNumber, ticketsViewModel.DeliveryDate, ticketsViewModel.InvoiceNumber,
                                                                                    currentPageNumber, pageSize, sortExpression, sortDirection, out totalRows);


                    var ticketViewModelList = ticketlist.Select(ticket => (TicketViewModel)ticket);
                    result.Tickets = ticketViewModelList.ToList();
                    result.TotalRows = totalRows;

                }
                response = ResponseMessage(Request.CreateResponse<TicketsViewModel>(HttpStatusCode.OK, result));

            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        [Authorize(Roles = "Company Admin, Company User, Customer Admin, Customer User")]
        [Route("GetDocumentContent")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDocumentContent(HttpRequestMessage request, [FromBody]  TicketViewModel ticketViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                var BBC = _ticketDataService.SearchDocument(ticketNumber: ticketViewModel.TicketNumber);
                string documentID = BBC.FirstOrDefault().Rows.FirstOrDefault().EDMDocumentID;
                byte[] BBD = _ticketDataService.RetrieveDocument(int.Parse(documentID));
                response = ResponseMessage(Request.CreateResponse<Array>(HttpStatusCode.OK, BBD));
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }

            return response;
        }

    }
}
