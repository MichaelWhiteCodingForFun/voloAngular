using POD.Entities;
using POD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.EntityFramework.DataServices
{
    //public class CustomerDataService : ICustomerDataService
    //{
    //    /// <summary>
    //    /// Create Customer
    //    /// </summary>
    //    /// <param name="customer"></param>
    //    public void CreateCustomer(Customer customer)
    //    {

    //    }

    //    /// <summary>
    //    /// Get Customers
    //    /// </summary>
    //    /// <param name="currentPageNumber"></param>
    //    /// <param name="pageSize"></param>
    //    /// <param name="sortExpression"></param>
    //    /// <param name="sortDirection"></param>
    //    /// <param name="totalRows"></param>
    //    /// <returns></returns>
    //    public List<Customer> GetCustomers(int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows)
    //    {


    //        List<Customer> customers = new List<Customer>()
    //        {
    //            new Customer() { CustomerID = 1, PayerAccountName = "Customer 1 LLC", PayerAccountNumber = "11", Email="customer1@a.com" },
    //        new Customer() { CustomerID = 2, PayerAccountName = "Customer 2 LLC", PayerAccountNumber = "22", Email="customer1@a.com" }, 
    //            new Customer() { CustomerID = 3, PayerAccountName = "Customer 3 LLC", PayerAccountNumber = "33", Email="customer1@a.com" }
    //        };

    //        totalRows = customers.Count();

    //        return customers.Skip((currentPageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
    //    }
    //}
}
