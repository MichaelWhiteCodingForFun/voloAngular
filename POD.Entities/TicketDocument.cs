using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace POD.Entities
{
    public enum TicketDocumentResultNames
    {
        SerialNo,
        BarCode,
        TicketNo,
        Depot,
        TicketDateDT,
        CustomerAcc,
        CustomerName,
        HaulierAcc,
        VehicleReg,
        SAPQuote,
        SAPInvoice,
        Payer,
        DateImported,
        EDMDocumentID
    }
    public enum TicketDocumentResultKeys
    {
        fldSerial_No,
        fldPOD_Ticket_No,
        fldSAP_Ticket_No,
        fldDepot,
        fldDate,
        fldCust_Acc,
        fldCust_Name,
        fldHaulier_Acc,
        fldVehicle_Reg,
        fldSAP_Quote,
        fldSAP_Invoice,
        fldPayer,
        fldDate_Imported,
        fldEDM_ID
    }
    public enum TicketDocumentSearchKeys
    {
        fldSAP_Ticket_No,
        fldCust_Acc,
        fldDate
    }
    //public enum TicketDocumentSearchKeys
    //{
    //    BarCode,
    //    SerialNo,
    //    TicketDateDT

    //}
    public class TicketDocument
    {
        public string SerialNo { get; set; }
        public string BarCode { get; set; }
        public string TicketNo { get; set; }
        public string Depot { get; set; }
        public string TicketDateDT { get; set; }
        public string CustomerAcc { get; set; }
        public string CustomerName { get; set; }
        public string HaulierAcc { get; set; }
        public string VehicleReg { get; set; }
        public string SAPQuote { get; set; }
        public string SAPInvoice { get; set; }
        public string Payer { get; set; }
        public string DateImported { get; set; }
        public string EDMDocumentID { get; set; }
    }
}
