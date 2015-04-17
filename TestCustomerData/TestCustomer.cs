using System;
using NUnit.Framework;
using Moq;
using CustomerService.Repository;
using ModelObjects;
using System.ServiceModel;


namespace TestCustomerData
{
    [TestFixture]
    public class TestCustomer
    {
        

        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentException))]
        public void GetCustomer_Exception_If_Passed_Invalid_customerId()
        {
            //Arrange
            Mock<ICustomer> mockCustomer = new Mock<ICustomer>();
            Mock<IArchivedCustomer> mockArchivedCustomer = new Mock<IArchivedCustomer>();
            Mock<ICustomerServiceData> mockCustomerServiceData = new Mock<ICustomerServiceData>();

            CustomerService.CustomerService objCustService = new CustomerService.CustomerService(mockCustomer.Object, mockArchivedCustomer.Object, mockCustomerServiceData.Object);

            //Act
            objCustService.GetCustomer(-1, false);           
            
        }        

        [Test]
        public void GetCustomer_If_Called_With_isCustomerArchived_As_True()
        {
            //Arrange
            Mock<ICustomer> mockCustomer = new Mock<ICustomer>();
            mockCustomer.Setup(t => t.GetCustomerByID(It.IsAny<int>())).Returns(new Customer() { Company = new Company(), Id = 100 });

            Mock<IArchivedCustomer> mockArchivedCustomer = new Mock<IArchivedCustomer>();
            Mock<ICustomerServiceData> mockCustomerServiceData = new Mock<ICustomerServiceData>();

            CustomerService.CustomerService objCustService = new CustomerService.CustomerService(mockCustomer.Object, mockArchivedCustomer.Object, mockCustomerServiceData.Object);

            //Act
            objCustService.GetCustomer(1, true);

            //Assert
            mockCustomer.Verify();
        }

        [Test]
        public void GetCustomer_If_Called_With_isCustomerArchived_As_False()
        {
            //Arrange
            Mock<ICustomer> mockCustomer = new Mock<ICustomer>();           

            Mock<ICustomerServiceData> mockCustomerServiceData = new Mock<ICustomerServiceData>();
            mockCustomerServiceData.Setup(t => t.GetCustomerByID(It.IsAny<int>())).Returns(new Customer() { Company = new Company(), Id = 100 });

            Mock<IArchivedCustomer> mockArchivedCustomer = new Mock<IArchivedCustomer>();
            

            CustomerService.CustomerService objCustService = new CustomerService.CustomerService(mockCustomer.Object, mockArchivedCustomer.Object, mockCustomerServiceData.Object);

            //Act
            objCustService.GetCustomer(1, false);

            //Assert
            mockCustomer.Verify();
        }


        [Test]
        public void GetCustomer_If_Called_When_The_Service_Is_Down()
        {
            //Arrange
            Mock<ICustomer> mockCustomer = new Mock<ICustomer>();

            Mock<ICustomerServiceData> mockCustomerServiceData = new Mock<ICustomerServiceData>();
            mockCustomerServiceData.Setup(t => t.GetCustomerByID(It.IsAny<int>())).Throws(new FaultException("Service Down"));

            Mock<IArchivedCustomer> mockArchivedCustomer = new Mock<IArchivedCustomer>();


            CustomerService.CustomerService objCustService = new CustomerService.CustomerService(mockCustomer.Object, mockArchivedCustomer.Object, mockCustomerServiceData.Object);

            //Act
            objCustService.GetCustomer(1, false);

            //Assert
            mockCustomerServiceData.Verify();
        }


        [Test]
        public void GetCustomer_If_Called_When_The_Service_Is_Down_And_Customer_Retrived_From_ArchivedRepository()
        {
            //Arrange
            Mock<ICustomer> mockCustomer = new Mock<ICustomer>();

            Mock<ICustomerServiceData> mockCustomerServiceData = new Mock<ICustomerServiceData>();
            mockCustomerServiceData.Setup(t => t.GetCustomerByID(It.IsAny<int>())).Throws(new FaultException("Service Down"));

            Mock<IArchivedCustomer> mockArchivedCustomer = new Mock<IArchivedCustomer>();


            CustomerService.CustomerService objCustService = new CustomerService.CustomerService(mockCustomer.Object, mockArchivedCustomer.Object, mockCustomerServiceData.Object);

            //Act
            objCustService.GetCustomer(1, false);

            //Assert
            mockArchivedCustomer.Verify();
        }

        [Test]        
        public void GetCustomer_Service_Working_Fine_With_Valid_Values_Passed()
        {
            //Arrange
            Mock<ICustomer> mockCustomer = new Mock<ICustomer>();
            Mock<IArchivedCustomer> mockArchivedCustomer = new Mock<IArchivedCustomer>();
            Mock<ICustomerServiceData> mockCustomerServiceData = new Mock<ICustomerServiceData>();
            mockCustomerServiceData.Setup(t => t.GetCustomerByID(It.IsAny<int>())).Returns(new Customer() { Company = new Company(), Id = 101, Firstname="Jon" });

            CustomerService.CustomerService objCustService = new CustomerService.CustomerService(mockCustomer.Object, mockArchivedCustomer.Object, mockCustomerServiceData.Object);

            //Act
            objCustService.GetCustomer(1, false);

            //Assert
            mockCustomerServiceData.Verify();            

        }

       
    }
}
