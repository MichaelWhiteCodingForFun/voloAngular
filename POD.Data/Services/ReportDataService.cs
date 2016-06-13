using POD.Data.Dapper;
using POD.Entities;
using POD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions.Mapper;
using System.Data;
using POD.Utilities;

namespace POD.Data.Services
{
    public class ReportDataService : BaseDataService, IReportDataService
    {
        static ReportDataService _instance;
        public ReportDataService()
        {
            _instance = this;
        }

        public static ReportDataService Instance
        {
            get { return _instance == null ? new ReportDataService() : _instance; }
        }

        public async Task<Guid> AddReportAsync(Report Report, bool SendEmail = false)
        {
            // this._db.Insert<Report>(Repo          
            if (SendEmail)
            {
                Task<bool> SendingTask = EmailManager.SendEmailFillBodyAsync(PODEnvironment.GetSetting("emailService:CompanyEmail"), "Report Subject", "Report", new object[] { Report.ReportTypeID, Report.TicketID, Report.Details, Report.IsSuccess }, new String[] { "C:\\Evolve\\PODPortal\\TestFiles\\Attach.txt" }  ); //remove file attachments
                Guid Guid = this.Insert<Report>(Report);
                return await Task<Guid>.Run(async () =>
                   {
                       await SendingTask;
                       return Guid;
                   });
            }
            else
            {
                return this.Insert<Report>(Report);
            }
        }

        public IEnumerable<Report> GetReports(Guid userID, int? reportTypeID, string details, int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows)
        {
            try
            {
                //test
                pageSize = 15;
                currentPageNumber = 1;
                List<Report> reports = new List<Report>();

                var offset = (currentPageNumber - 1) * pageSize;
                var param = new DynamicParameters();

                param.Add("@UserID", userID, dbType: DbType.Guid, direction: ParameterDirection.Input);
                param.Add("@ReportTypeID", reportTypeID, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@Details", details, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Offset", offset, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@PageSize", pageSize, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@SortExpression", sortExpression, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@SortDirection", sortDirection, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var context = PODFactory.GetContext(true))
                {
                    var query = context.Database.Connection.QueryMultiple("[GetReports]", param, commandType: CommandType.StoredProcedure);
                    reports = query.Read<Report>().ToList();

                    totalRows = param.Get<int>("RowCount");
                }

                return reports;

            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }
    }
}
