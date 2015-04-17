using ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Repository
{
    public class CustomerServiceData : ICustomerServiceData
    {
        // Code for aclling the external web service will go here...
        public ModelObjects.Customer GetCustomerByID(int customerId)
        {
            return new Customer()
            {
                Company = new Company() { Classification = Classification.Gold, Id = 1, Name = "Test Company" },
                Id = 1,
                Firstname = "FailoverCustomerFirstName",
                Surname = "FailoverCustomerLastName",
                CreditLimit = 100,
                DateOfBirth = Convert.ToDateTime("01/01/1901"),
                EmailAddress = "test1@test.com",
            };
        }
    }
}
