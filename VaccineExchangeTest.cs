using CostelloClassLibrary;
using CostelloMVCWebProject.Controllers;
using CostelloMVCWebProject.Models;
using CostelloMVCWebProject.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CostelloTestProject
{
    public class VaccineExchangeTest
    {
        private readonly Mock<IVaccineExchangeRepo> mockVaccineExchangeRepo;
        private readonly Mock<IFacilityRepo> mockFacilityRepo;
        private readonly Mock<IFacilityInventoryRepo> mockFacilityInventoryRepo;
        private readonly VaccineExchangeController controller;

        public VaccineExchangeTest()
        {
            mockVaccineExchangeRepo = new Mock<IVaccineExchangeRepo>();
            mockFacilityRepo = new Mock<IFacilityRepo>();
            mockFacilityInventoryRepo = new Mock<IFacilityInventoryRepo>();
            controller = new VaccineExchangeController(mockVaccineExchangeRepo.Object, mockFacilityRepo.Object, mockFacilityInventoryRepo.Object);
        }
            [Fact]
            public void ShouldMakeVaccineExchange()//happy path all goes well
            {


                //Arrange
               
        
                MakeVaccineExchangeViewModel newvacex = new MakeVaccineExchangeViewModel
                {
                    ExchangedDate = new DateTime (2021, 9, 26),
                    ExchangedDoses = 999,
                    SendingFacilityID = 12,
                    ReceivingFacilityID = 69, //new DateTime(2021, 9, 25),
                    VaccineID = 1001
                };
                DateTime expectedexdate = new DateTime(2021, 9, 26);

            FacilityInventory sendingfacilityinventory = new FacilityInventory(newvacex.SendingFacilityID, newvacex.VaccineID);
            sendingfacilityinventory.CurrentInventory = 1000;

                mockFacilityInventoryRepo.Setup(m=>m.FindFacilityInventory(newvacex.SendingFacilityID, newvacex.VaccineID)).Returns(sendingfacilityinventory);

            FacilityInventory receivingfacilityinventory = new FacilityInventory(newvacex.ReceivingFacilityID, newvacex.VaccineID);
            receivingfacilityinventory.CurrentInventory = 1;

            mockFacilityInventoryRepo.Setup(m => m.FindFacilityInventory(newvacex.ReceivingFacilityID, newvacex.VaccineID)).Returns(receivingfacilityinventory);

            VaccineExchange editedvacex = new VaccineExchange();//(viewmodel.FacilityID, viewmodel.FacilityID, viewmodel.StartDate.Value, viewmodel.NumberOfVaccinesShipped.Value, viewmodel.EndDate.Value);

                int mockvaccineexchangeid = 9999;

                mockVaccineExchangeRepo.Setup(m => m.MakeVaccineExhange(It.IsAny<VaccineExchange>())).Returns(mockvaccineexchangeid);

                mockVaccineExchangeRepo.Setup(m => m.MakeVaccineExhange(It.IsAny<VaccineExchange>())).Returns(mockvaccineexchangeid).Callback<VaccineExchange>(vs => editedvacex = vs);
                //Act
                controller.MakeVaccineExchange(newvacex);

            int expectedCurrentInventoryforSender = 1;
            int expectedCurrentInventoryforReceiver = 1000;
                //VaccineShipment newaddedvaccineshipment = null;


                //Assert

                mockVaccineExchangeRepo.Verify(
                    m => m.MakeVaccineExhange(It.IsAny<VaccineExchange>()), Times.Exactly(1));
                Assert.Equal(expectedexdate, editedvacex.ExchangedDate);
            Assert.Equal(mockvaccineexchangeid, editedvacex.VaccineExchangeID);
            Assert.Equal(expectedCurrentInventoryforSender, sendingfacilityinventory.CurrentInventory);
            Assert.Equal(expectedCurrentInventoryforReceiver, receivingfacilityinventory.CurrentInventory);
        }
        [Fact]
        public void ShouldNotMakeVacccineExchange()
        {
            mockFacilityRepo.Setup(m => m.ListOfAllFacilities()).Returns(new List<Facility>());

            MakeVaccineExchangeViewModel newvacex = new MakeVaccineExchangeViewModel
            {
                ExchangedDate = new DateTime(2021, 9, 26),
                ExchangedDoses = 999,
                SendingFacilityID = 12,
                ReceivingFacilityID = 69, //new DateTime(2021, 9, 25),
                VaccineID = 1001
            };
            DateTime expectedexdate = new DateTime(2021, 9, 26);

            FacilityInventory sendingfacilityinventory = new FacilityInventory(newvacex.SendingFacilityID, newvacex.VaccineID);
            sendingfacilityinventory.CurrentInventory = 998;

            mockFacilityInventoryRepo.Setup(m => m.FindFacilityInventory(newvacex.SendingFacilityID, newvacex.VaccineID)).Returns(sendingfacilityinventory);

            //FacilityInventory receivingfacilityinventory = new FacilityInventory(newvacex.ReceivingFacilityID, newvacex.VaccineID);
            //receivingfacilityinventory.CurrentInventory = 1;

            //mockFacilityInventoryRepo.Setup(m => m.FindFacilityInventory(newvacex.ReceivingFacilityID, newvacex.VaccineID)).Returns(receivingfacilityinventory);

            /*VaccineExchange editedvacex = new VaccineExchange();*///(viewmodel.FacilityID, viewmodel.FacilityID, viewmodel.StartDate.Value, viewmodel.NumberOfVaccinesShipped.Value, viewmodel.EndDate.Value);

            int mockvaccineexchangeid = 9999;

            mockVaccineExchangeRepo.Setup(m => m.MakeVaccineExhange(It.IsAny<VaccineExchange>())).Returns(mockvaccineexchangeid);

            //mockVaccineExchangeRepo.Setup(m => m.MakeVaccineExhange(It.IsAny<VaccineExchange>())).Returns(mockvaccineexchangeid).Callback<VaccineExchange>(vs => editedvacex = vs);
            //Act
            controller.MakeVaccineExchange(newvacex);

            int expectedCurrentInventoryforSender = 998;
            //int expectedCurrentInventoryforReceiver = 1000;
            //VaccineShipment newaddedvaccineshipment = null;
            string expectedErrorMessage = "Number of Vaccines is more than is available at the Sending Facility";
                string actualErrorMessage = controller.ModelState["InsufficientVaccinesError"].Errors[0].ErrorMessage;

            //Assert

            mockVaccineExchangeRepo.Verify(
                m => m.MakeVaccineExhange(It.IsAny<VaccineExchange>()), Times.Never);
            //Assert.Equal(expectedexdate, editedvacex.ExchangedDate);
            //Assert.Equal(mockvaccineexchangeid, editedvacex.VaccineExchangeID);
            Assert.Equal(expectedCurrentInventoryforSender, sendingfacilityinventory.CurrentInventory);
            Assert.Equal(expectedErrorMessage, actualErrorMessage);
            //Assert.Equal(expectedCurrentInventoryforReceiver, receivingfacilityinventory.CurrentInventory);
        }

    }
}
