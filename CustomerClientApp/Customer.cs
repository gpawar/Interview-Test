using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomerService;
using Microsoft.Practices.Unity;
using CustomerService.Repository;


namespace CustomerClientApp
{
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
        }

        private void btnGetCustomer_Click(object sender, EventArgs e)
        {
            try
            {

                IUnityContainer unitycontainer = new UnityContainer();
                unitycontainer.RegisterType<ICustomer, CustomerRepository>();
                unitycontainer.RegisterType<IArchivedCustomer, ArchivedDataRepository>();
                unitycontainer.RegisterType<ICustomerServiceData, CustomerServiceData>();


                CustomerService.CustomerService objCustService = unitycontainer.Resolve<CustomerService.CustomerService>();
                objCustService.GetCustomer(1, true);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
