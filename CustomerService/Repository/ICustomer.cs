using ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Repository
{
    public interface ICustomer
    {
        Customer GetCustomerByID(int customerId);
    }
}
