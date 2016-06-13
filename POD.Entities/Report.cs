using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Entities
{
    public enum ReportType
    {
        FileImpor = 1,
        MissingPOD,
        MistakeinPOD,
        Login
    }

    public class Report
    {
        public Guid ID {get; set; }
        public int ReportTypeID { get; set; }    
        public DateTime CreatedDateUtc { get; set; }
        public string Details { get; set; }
        public Guid? TicketID { get; set; }        
        public Guid? UserID { get; set; }
        public bool IsSuccess {get; set;}
        public string CreatorUser { get; set; }
        public Guid? OrganizationID { get; set; }
    }

    public class ReportrMapper : ClassMapper<Report>
    {
        public ReportrMapper()
        {
            base.Map(u => u.CreatorUser).Ignore();           
            base.AutoMap();
        }
    }
}
