using POD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.Entities;
using POD.Data.Dapper;
using Dapper;
using DapperExtensions.Mapper;
using System.Data;
using POD.Data.EDMOnlineServiceReference;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using POD.Utilities;

namespace POD.Data.Services
{
    public class TicketDataService : BaseDataService, ITicketDataService
    {
        static TicketDataService _instance;

        public TicketDataService()
        {
            _instance = this;
        }
        public static TicketDataService Instance
        {
            get { return _instance == null ? new TicketDataService() : _instance; }
        }
        public Guid InsertTicket(Ticket ticket)
        {
            return base.Insert<Ticket>(ticket);
        }
        public bool UpdateTicket(Ticket ticket)
        {
            return base.Update<Ticket>(ticket);
        }
        public Ticket GetTicket(int ticketNumber)
        {
            string sql = "SELECT * from [Ticket] Where  TicketNumber='" + ticketNumber + "'";
            List<Ticket> tickets = new List<Ticket>();
            using (var context = PODFactory.GetContext(true))
            {
                var result = context.Database.Connection.Query<Ticket>(sql).FirstOrDefault();

                if (result != null)
                {
                    return result;
                }
                return null;
            }
        }
        public IEnumerable<Ticket> GetTickets(Guid organizationID, string accontNumber, string accountName, int? ticketNumber, DateTime? deliveryDate, string invoiceNumber, int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows)
        {
           
            try
            {
                //test
                pageSize = 15;
                currentPageNumber = 1;
                List<Ticket> tickets = new List<Ticket>();

                var offset = (currentPageNumber - 1) * pageSize;
                var param = new DynamicParameters();

                param.Add("@OrganizationID", organizationID, dbType: DbType.Guid, direction: ParameterDirection.Input);
                param.Add("@AccontNumber", accontNumber, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@AccountName", accountName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TicketNumber", ticketNumber, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@DeliveryDate", deliveryDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                param.Add("@InvoiceNumber", invoiceNumber, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Offset", offset, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@PageSize", pageSize, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@SortExpression", sortExpression, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@SortDirection", sortDirection, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var context = PODFactory.GetContext(true))
                {
                    var query = context.Database.Connection.QueryMultiple("[GetTickets]", param, commandType: CommandType.StoredProcedure);
                    tickets = query.Read<Ticket>().ToList();

                    totalRows = param.Get<int>("RowCount");
                }

                return tickets;
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }

        public List<TicketTable> SearchDocument(string accountNumber = null, int? ticketNumber = null, DateTime? deliveryDate = null)
        {
            #region Request Parameters

            Credentials credentials = new Credentials()
            {
                AppUsername = PODEnvironment.GetSetting("AppUsername"),
                ClientUsername = PODEnvironment.GetSetting("ClientUsername"),
                AppPassword = PODEnvironment.GetSetting("AppPassword")
            };

            string companyID = PODEnvironment.GetSetting("companyID");
            string documentType = PODEnvironment.GetSetting("documentType");
            DataFormat dataFormat = DataFormat.JSON;

            //fill the required columns to be returned from the service
            List<ResultField> resultDatas = new List<ResultField>();
            List<TicketDocumentResultNames> FieldNamesList = Enum.GetValues(typeof(TicketDocumentResultNames)).Cast<TicketDocumentResultNames>().ToList();
            List<TicketDocumentResultKeys> FieldKeysList = Enum.GetValues(typeof(TicketDocumentResultKeys)).Cast<TicketDocumentResultKeys>().ToList();
            var zippedFields = FieldNamesList.Zip(FieldKeysList, (n, w) => new { Value = n, Key = w });
            foreach (var zippedField in zippedFields)
            {
                resultDatas.Add(new ResultField() { FieldName = zippedField.Key.ToString(), FieldAlias = zippedField.Value.ToString() });
            }

            //fill the search criteria fields
            List<DataField> searchDatas = new List<DataField>();
            if (!String.IsNullOrWhiteSpace(accountNumber))
            {
                DataField searchData = new DataField() { FieldName = TicketDocumentSearchKeys.fldCust_Acc.ToString(), FieldValue = accountNumber/* "143016" */};              
                searchDatas.Add(searchData);
            }
            if (ticketNumber.HasValue)
            {
                DataField searchData = new DataField() { FieldName = TicketDocumentSearchKeys.fldSAP_Ticket_No.ToString() , FieldValue = ticketNumber.Value.ToString()/* "143016" */};
                searchDatas.Add(searchData);
            }
            if (deliveryDate.HasValue)
            {
                DataField searchData = new DataField() { FieldName = TicketDocumentSearchKeys.fldDate.ToString() , FieldValue = deliveryDate.Value.ToString("dd/MM/yyyy HH:mm:ss")/* "143016" */};
                searchDatas.Add(searchData);
            }
            //DataField searchData2 = new DataField() { FieldName = "fldCust_Acc", FieldValue = null };
            //searchDatas.Add(searchData2);


            #region Zibil
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldSerial_No.ToString(), FieldAlias = TicketDocumentFieldNames.SerialNo.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldPOD_Ticket_No.ToString(), FieldAlias = TicketDocumentFieldNames.BarCode.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldSAP_Ticket_No.ToString(), FieldAlias = TicketDocumentFieldNames.TicketNo.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldDepot.ToString(), FieldAlias = TicketDocumentFieldNames.Depot.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldDate.ToString(), FieldAlias = TicketDocumentFieldNames.TicketDateDT.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldCust_Acc.ToString(), FieldAlias = TicketDocumentFieldNames.CustomerAcc.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldCust_Name.ToString(), FieldAlias = TicketDocumentFieldNames.CustomerName.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldHaulier_Acc.ToString(), FieldAlias = TicketDocumentFieldNames.HaulierAcc.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldVehicle_Reg.ToString(), FieldAlias = TicketDocumentFieldNames.VehicleReg.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldSAP_Quote.ToString(), FieldAlias = TicketDocumentFieldNames.SAPQuote.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldSAP_Invoice.ToString(), FieldAlias = TicketDocumentFieldNames.SAPInvoice.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldPayer.ToString(), FieldAlias = TicketDocumentFieldNames.Payer.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldDate_Imported.ToString(), FieldAlias = TicketDocumentFieldNames.DateImported.ToString() });
            //resultDatas.Add(new ResultField() { FieldName = TicketDocumentFieldKeys.fldEDM_ID.ToString(), FieldAlias = TicketDocumentFieldNames.EDMDocumentID.ToString() });
            #endregion

            #endregion


            EDMOnlineServiceReference.DocumentManagementClient client = new DocumentManagementClient();
            Document doc = new EDMOnlineServiceReference.Document();

            try
            {
                List<TicketTable> ticketDocuments = new List<TicketTable>();          
                var response = client.DocumentSearch(credentials, companyID, documentType, searchDatas, resultDatas, dataFormat);
                var responseData = JObject.Parse(response.Data).ToString();
                ticketDocuments = JsonConvert.DeserializeAnonymousType(responseData, new { Tables = (List<TicketTable>)null }).Tables;

                return ticketDocuments;
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }

        public byte[] RetrieveDocument(int documentID)
        {
            Credentials credentials = new Credentials()
            {
                AppUsername = PODEnvironment.GetSetting("AppUsername"),
                ClientUsername = PODEnvironment.GetSetting("ClientUsername"),
                AppPassword = PODEnvironment.GetSetting("AppPassword")
            };
            string companyID = PODEnvironment.GetSetting("companyID");
            string documentType = PODEnvironment.GetSetting("documentType");
            string conversionFlag = "PDF";
            EDMOnlineServiceReference.DocumentManagementClient client = new DocumentManagementClient();
            Document doc = new EDMOnlineServiceReference.Document();

            try
            {
                var response = client.DocumentRetrieve(credentials, companyID, documentID, documentType, conversionFlag);
                byte[] responseData = response.documentContent;
                return responseData;
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }

        }

    }
}
