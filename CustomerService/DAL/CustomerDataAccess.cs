using ModelObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CustomerService.DAL
{
    public class CustomerDataAccess 
    {

        public void AddCustomer(Customer customer)
        {
            var connectionString =   ConfigurationManager.AppSettings["appDatabase"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "uspAddCustomer"
                };

                var firstNameParameter = new SqlParameter("@Firstname", SqlDbType.VarChar, 50) { Value = customer.Firstname };
                command.Parameters.Add(firstNameParameter);
                var surnameParameter = new SqlParameter("@Surname", SqlDbType.VarChar, 50) { Value = customer.Surname };
                command.Parameters.Add(surnameParameter);
                var dateOfBirthParameter = new SqlParameter("@DateOfBirth", SqlDbType.DateTime) { Value = customer.DateOfBirth };
                command.Parameters.Add(dateOfBirthParameter);
                var emailAddressParameter = new SqlParameter("@EmailAddress", SqlDbType.VarChar, 50) { Value = customer.EmailAddress };
                command.Parameters.Add(emailAddressParameter);
                var hasCreditLimitParameter = new SqlParameter("@HasCreditLimit", SqlDbType.Bit) { Value = customer.HasCreditLimit };
                command.Parameters.Add(hasCreditLimitParameter);
                var creditLimitParameter = new SqlParameter("@CreditLimit", SqlDbType.Int) { Value = customer.CreditLimit };
                command.Parameters.Add(creditLimitParameter);
                var companyIdParameter = new SqlParameter("@CompanyId", SqlDbType.Int) { Value = customer.Company.Id };
                command.Parameters.Add(companyIdParameter);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Customer GetArchivedCustomer(int customerId)
        {
            Customer objCust = new Customer();
            objCust.Company = new Company();

            try
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["appDatabase"].ConnectionString;
                //var connectionString = ConfigurationManager.AppSettings["appDatabase"].ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetCustDetails", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CustomerID", customerId));

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            objCust.Id = Convert.ToInt16(rdr["Id"]);
                            objCust.CreditLimit = Convert.ToInt16(rdr["CreditLimit"]);
                            objCust.DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]);
                            objCust.EmailAddress = rdr["EmailAddress"].ToString();
                            objCust.Firstname = rdr["Firstname"].ToString();
                            objCust.Surname = rdr["Surname"].ToString();

                            objCust.Company.Id = Convert.ToInt16(rdr["CompanyId"]);
                            objCust.Company.Classification = (Classification)(rdr["Classification"]);
                            objCust.Company.Name = (rdr["CompanyName"]).ToString();
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return objCust;
                       

         }

        public Customer GetCustomerFromFailover(int customerId)
        {

            Customer objCust = new Customer();
            objCust.Company = new Company();

            try
            {

                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["appDatabase"].ConnectionString;

                //var connectionString = ConfigurationManager.AppSettings["appDatabase"].ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetCustDetails", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CustomerID", customerId));

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            objCust.Id = Convert.ToInt16(rdr["Id"]);
                            objCust.CreditLimit = Convert.ToInt16(rdr["CreditLimit"]);
                            objCust.DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]);
                            objCust.EmailAddress = rdr["EmailAddress"].ToString();
                            objCust.Firstname = rdr["Firstname"].ToString();
                            objCust.Surname = rdr["Surname"].ToString();

                            objCust.Company.Id = Convert.ToInt16(rdr["CompanyId"]);
                            objCust.Company.Classification = (Classification)(rdr["Classification"]);
                            objCust.Company.Name = (rdr["CompanyName"]).ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return objCust;

        }



    }
}
