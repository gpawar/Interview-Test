using CustomerService.Repository;
using ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Practices.Unity;
using System.ServiceModel;
namespace CustomerService
{
    public class CustomerService
    {
        readonly ICustomer _customerRepository;
        readonly IArchivedCustomer _customerarchivedRepository;
        readonly ICustomerServiceData _customerServiceData;
       

        [InjectionConstructor]  
        public CustomerService(ICustomer customerRepository, 
                               IArchivedCustomer customerarchivedRepository,
                               ICustomerServiceData customerServiceData)
        {
            _customerRepository = customerRepository;
            _customerarchivedRepository = customerarchivedRepository;
            _customerServiceData = customerServiceData;
        }

        #region "Add Customer"

        
        public bool AddCustomer(string firname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            //if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            //{
            //    return false;
            //}

            //if (!email.Contains("@") && !email.Contains("."))
            //{
            //    return false;
            //}

            //var now = DateTime.Now;
            //int age = now.Year - dateOfBirth.Year;
            //if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            //if (age < 21)
            //{
            //    return false;
            //}

            //var companyRepository = new CompanyRepository();
            //var company = companyRepository.GetById(companyId);

            //var customer = new Customer
            //{
            //    Company = company,
            //    DateOfBirth = dateOfBirth,
            //    EmailAddress = email,
            //    Firstname = firname,
            //    Surname = surname
            //};

            //if (company.Name == "VeryImportantClient")
            //{
            //    // Skip credit check
            //    customer.HasCreditLimit = false;
            //}
            //else if (company.Name == "ImportantClient")
            //{
            //    // Do credit check and double credit limit
            //    customer.HasCreditLimit = true;
            //    using (var customerCreditService = new CustomerCreditServiceClient())
            //    {
            //        var creditLimit = customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
            //        creditLimit = creditLimit * 2;
            //        customer.CreditLimit = creditLimit;
            //    }
            //}
            //else
            //{
            //    // Do credit check
            //    customer.HasCreditLimit = true;
            //    using (var customerCreditService = new CustomerCreditServiceClient())
            //    {
            //        var creditLimit = customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
            //        customer.CreditLimit = creditLimit;
            //    }
            //}

            //if (customer.HasCreditLimit && customer.CreditLimit < 500)
            //{
            //    return false;
            //}

            //CustomerDataAccess.AddCustomer(customer);

            return true;
        }

        #endregion

        //Startup Method
        public Customer GetCustomer(int Id, bool isCustomerArchived)
        {
            Customer objCustomer = null;
            try
            {
                if (Id <= 0)
                {
                    throw new ArgumentException("Customer Id is not valid");
                }

                string intrequestCounter = ConfigurationManager.AppSettings["RequestCounter"].ToString();

                //If client passes isCustomerArchived = true then go and fetch the client details from archived
                if (isCustomerArchived)
                {
                    objCustomer = _customerRepository.GetCustomerByID(Id);
                }

                DateTime Starttime = System.DateTime.Now;


                //If Customer is still null then get it from Service
                //Loop has been put so that we try fetching the data from service till reqCount> 10 OR TotalMinutes > 10 mins
                if(objCustomer == null)
                {
                    for (int reqCount = 0; reqCount < (Convert.ToInt16(intrequestCounter) - 1); reqCount++)
                    {
                        try
                        {
                            objCustomer = _customerServiceData.GetCustomerByID(Id);
                        }
                        catch (FaultException)
                        {// Dont do anything until retry till the reqCount elapsed OR Timer above 10 mins
                        }
                        
                        if (objCustomer != null)
                            break;

                        if (System.DateTime.Now.Subtract(Starttime).TotalMinutes > 10)
                            break;
                    }
                }

                //If Customer is still null then get Customer details from Archived DB
                if (objCustomer == null)
                {
                    objCustomer = _customerarchivedRepository.GetCustomerByID(Id);
                }               

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objCustomer;
        }

    }
}
