using CustomerService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Repository
{
    public class ArchivedDataRepository : IArchivedCustomer
    {
        CustomerDataAccess objDataAccess = new CustomerDataAccess();

        public ModelObjects.Customer GetCustomerByID(int customerId)
        {
            return objDataAccess.GetArchivedCustomer(customerId);
        }
    }
}
