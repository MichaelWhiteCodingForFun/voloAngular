using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POD.Portal.Models
{
    public class BaseViewModel
    {
        public bool ReturnStatus { get; set; }


        public int PageSize { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }


    //public class BaseViewModelList : List<BaseViewModel>
    //{
    //    public int PageSize { get; set; }
    //    public string SortExpression { get; set; }
    //    public string SortDirection { get; set; }
    //    public int CurrentPageNumber { get; set; }
    //    public int TotalPages { get; set; }
    //    public int TotalRows { get; set; }
    //}
}