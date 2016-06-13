using POD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Interfaces
{
    public interface IReportDataService
    {
        Task<Guid>  AddReportAsync(Report Report, bool SendEmail = false);

        IEnumerable<Report> GetReports(Guid userID, int? reportTypeID, string details, int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows);
    }
}
