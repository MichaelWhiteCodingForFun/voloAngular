using Ninject;
using POD.Entities;
using POD.Interfaces;
using POD.Portal.Helpers;
using POD.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace POD.Portal.Controllers.WebApi
{
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        [Inject]
        public IReportDataService _reportDataService { get; set; }             
        public ReportController(IReportDataService reportDataService)
        {            
            _reportDataService = reportDataService;            
        }

        [Route("reports")]
        [HttpPost]
        public async Task<IHttpActionResult> GetReports(HttpRequestMessage request, [FromBody] ReportsViewModel reportsViewModel)
        {
            //throw new Exception("qas");
            IHttpActionResult response = null;
            try
            {
                ReportsViewModel result = new ReportsViewModel();
                UserIdentityInfo userinfo = UserHelper.GetUserInfo(this);
                if (userinfo != null)
                {
                    int currentPageNumber = reportsViewModel.CurrentPageNumber;
                    int pageSize = reportsViewModel.PageSize;
                    string sortExpression = reportsViewModel.SortExpression;
                    string sortDirection = reportsViewModel.SortDirection;
                    int totalRows;
                    IEnumerable<Report> reportsList = _reportDataService.GetReports(userinfo.ID, null, null, currentPageNumber, pageSize, sortExpression, sortDirection, out totalRows);

                    IEnumerable<ReportViewModel> reportViewModelList = reportsList.Select(user => (ReportViewModel)user);
                    result.Reports = reportViewModelList.ToList();
                    result.TotalRows = totalRows;
                }
                response = ResponseMessage(Request.CreateResponse<ReportsViewModel>(HttpStatusCode.OK, result));
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        public async Task<IHttpActionResult> AddReport(ReportViewModel Report)
        {
            Report ReportEntity = new Entities.Report();
            ReportEntity.Details = Report.Details;
            Guid reportID = await _reportDataService.AddReportAsync(ReportEntity, true);
            IHttpActionResult response;
            if (reportID != Guid.Empty)
            {
                response = ResponseMessage(Request.CreateResponse<Guid>(HttpStatusCode.OK, reportID));
            }
            else
            {
                response = BadRequest();
            }
            return response;
        }
    }


}
