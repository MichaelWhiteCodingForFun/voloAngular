using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.Entities;

namespace POD.Portal.Models
{
    public class ReportViewModel
    {
        public Guid ID {get; set; }
        public int ReportTypeID { get; set; }    
        public DateTime CreatedDateUtc { get; set; }
        public string Details { get; set; }
        public Guid? TicketID { get; set; }
        public Guid? UserID { get; set; }
        public Guid? OrganizationID { get; set; }
        public bool IsSuccess {get; set;}

        public static implicit operator ReportViewModel(Report reportEntity)
        {
            return new ReportViewModel
            {
                ID = reportEntity.ID,
                UserID = reportEntity.UserID,
                OrganizationID = reportEntity.OrganizationID,
                CreatedDateUtc = reportEntity.CreatedDateUtc,
                Details = reportEntity.Details,
                IsSuccess = reportEntity.IsSuccess,
                ReportTypeID = reportEntity.ReportTypeID,
                TicketID = reportEntity.TicketID
            };
        }

        public static implicit operator Report(ReportViewModel reportViewModel)
        {
            return new Report
            {
                ID = reportViewModel.ID,
                UserID = reportViewModel.UserID,
                OrganizationID = reportViewModel.OrganizationID,
                CreatedDateUtc = reportViewModel.CreatedDateUtc,
                Details = reportViewModel.Details,
                IsSuccess = reportViewModel.IsSuccess,
                ReportTypeID = reportViewModel.ReportTypeID,
                TicketID = reportViewModel.TicketID
            };
        }

    }

    public class ReportsViewModel : BaseViewModel
    {
        public List<ReportViewModel> Reports { get; set; }
        public int CurrentPageNumber {get; set;}
        public int ReportTypeID {get; set;}
        public string SrchString { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int pageSize { get; set; }
    }
}
