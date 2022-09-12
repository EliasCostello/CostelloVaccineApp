using System.Collections.Generic;
using Xunit;
using CostelloMVCWebProject.Models;
using System;
using CostelloMVCWebProject.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CostelloClassLibrary;
using CostelloMVCWebProject.ViewModels;

namespace CostelloTestProject
{
    public class VaccineShipmentTest
    {
        private readonly Mock<IVaccineShipmentRepo> mockVaccineShipmentRepo;
        private readonly Mock<IFacilityRepo> mockFacilityRepo;
        private readonly Mock<IApplicationUserRepo> mockApplicationUserRepo;
        private readonly Mock<IFacilityInventoryRepo> mockFacilityInventoryRepo;
        private readonly VaccineShipmentController controller;
        public VaccineShipmentTest()
        {
            mockVaccineShipmentRepo = new Mock<IVaccineShipmentRepo>();
            mockFacilityRepo = new Mock<IFacilityRepo>();
            mockApplicationUserRepo = new Mock<IApplicationUserRepo>();
            mockFacilityInventoryRepo = new Mock<IFacilityInventoryRepo>();
            controller = new VaccineShipmentController(mockVaccineShipmentRepo.Object, mockFacilityRepo.Object, mockApplicationUserRepo.Object, mockFacilityInventoryRepo.Object);
        }

        [Fact]
        public void ShouldFindVaccineShipment()
        {
            //Arrange
            VaccineShipment expectedVaccineShipment = new VaccineShipment(11,22,new DateTime(2021,1,1),33);
            int? vaccineshipmentid = 999;
            mockFacilityRepo.Setup(m => m.ListOfAllFacilities()).Returns(new List<Facility>());
            mockVaccineShipmentRepo.Setup(m => m.FindVaccineShipment(vaccineshipmentid)).Returns(expectedVaccineShipment);
            //Act
            ViewResult viewresult = controller.EditVaccineShipment(vaccineshipmentid) as ViewResult;



            VaccineShipment actualVaccineShipment = viewresult.Model as VaccineShipment;
            //Assert
            Assert.Equal(actualVaccineShipment, expectedVaccineShipment);
        }
        [Fact]
        public void ShouldEditVaccineShipment()
        {
            //Arrange
            VaccineShipment originalVaccineShipment = new VaccineShipment(11, 22, new DateTime(2021, 1, 1), 33);

            VaccineShipment modifiedVaccineShipment = new VaccineShipment(110, 220, new DateTime(2021, 1, 1), 330);

            mockVaccineShipmentRepo.Setup(m => m.EditVaccineShipment(modifiedVaccineShipment)).Callback<VaccineShipment>(vs => originalVaccineShipment = vs);
            //Act


            controller.EditVaccineShipment(modifiedVaccineShipment);
            //Assert
            int expectedmodifiedfacilityid = 110;
            Assert.Equal(expectedmodifiedfacilityid, originalVaccineShipment.FacilityID);
        }

        [Fact]
        public void ShouldAddVaccineShipment()//happy path all goes well
        {


            //Arrange
            string mockFacilityAdminID = "M12345";
            mockApplicationUserRepo.Setup(m => m.FindUserID()).Returns(mockFacilityAdminID);
            AddVaccineShipmentViewModel viewmodel = new AddVaccineShipmentViewModel
            {
                FacilityID = 1,
                VaccineID = 101,
                StartDate = new DateTime(2021, 9, 19),
                EndDate = null, //new DateTime(2021, 9, 25),
                NumberOfVaccinesShipped = 1000
            };

            DateTime expectedenddate = new DateTime(2021, 9, 26);

            FacilityInventory facilityinventory = new FacilityInventory(viewmodel.FacilityID, viewmodel.VaccineID);
            facilityinventory.CurrentInventory = 999;

            mockFacilityInventoryRepo.Setup(m => m.FindFacilityInventory(viewmodel.FacilityID, viewmodel.VaccineID)).Returns(facilityinventory);

            int expectedupdatedfacilityinventory = 1999;

            VaccineShipment vaccineshipment = new VaccineShipment();//(viewmodel.FacilityID, viewmodel.FacilityID, viewmodel.StartDate.Value, viewmodel.NumberOfVaccinesShipped.Value, viewmodel.EndDate.Value);

            int mockvaccineshipmentid = 9999;

            mockVaccineShipmentRepo.Setup(m => m.AddVaccineShipment(It.IsAny<VaccineShipment>())).Returns(mockvaccineshipmentid);

            mockVaccineShipmentRepo.Setup(m => m.AddVaccineShipment(It.IsAny<VaccineShipment>())).Returns(mockvaccineshipmentid).Callback<VaccineShipment>(vs => vaccineshipment = vs);
            //Act
            controller.AddVaccineShipment(viewmodel);

            //VaccineShipment newaddedvaccineshipment = null;

            
            //Assert

            mockVaccineShipmentRepo.Verify(
                m => m.AddVaccineShipment(It.IsAny<VaccineShipment>()), Times.Exactly(1));
            Assert.Equal(expectedenddate, vaccineshipment.EndDate);
            //Assert.Equal(mockvaccineshipmentid, vaccineshipment.VaccineShipmentID);
            Assert.Equal(mockFacilityAdminID, vaccineshipment.FacilityAdminID);

            Assert.Equal(facilityinventory.CurrentInventory, expectedupdatedfacilityinventory);
        }
        //sad path: ShoudlNotAddVaccineShipment

        [Fact]//TDD: Test Driven Development ( Incremental) Red-Green

        public void ShouldListAllVaccineShipments()
        {
            //AAA Testing: We expect something to happen; Compare expected against Actual
            //1.Arrange

            int expectednumberofshipments = 6;
            int actualnumberofshipments = 0;
            List<VaccineShipment> mockVaccineShipments = CreateMockVaccineShipmentData();
            //Inject mock data into the controller method
            mockVaccineShipmentRepo.Setup(m => m.ListAllVaccineShipments()).Returns(mockVaccineShipments);
            //2.Act (Actual)
            SearchVaccineShipmentsViewModel viewmodel = new SearchVaccineShipmentsViewModel();
            ViewResult result = controller.ListAllVaccineShipments() as ViewResult; //get back a result
            //look inside the returned result object
            List<VaccineShipment> actualvaccineshipments = result.Model as List<VaccineShipment>;
            //count the list
            actualnumberofshipments = actualvaccineshipments.Count;
            //3. Assert (Fails->Succeeds)
            Assert.Equal(expectednumberofshipments, actualnumberofshipments);
        }

        //SearchVaccineShipment
        //1. Date (StartDate and EndDate)
        //2. Name (Vaccine)
        //3. Facility Name / Address
        [Fact]
        public void ShouldSearchVAccineShipmentsByStartDate()
        {
            //AAA Testing: We expect something to happen; Compare expected against Actual
            //1.Arrange

            int expectednumberofshipments = 3;
            int actualnumberofshipments = 0;
            List<VaccineShipment> mockVaccineShipments = CreateMockVaccineShipmentData();

            DateTime? startdate = new DateTime(2021, 4, 11);
            SearchVaccineShipmentsViewModel viewmodel = new SearchVaccineShipmentsViewModel();
            viewmodel.StartDate = startdate;
            viewmodel.EndDate = null;
            viewmodel.FacilityID = null;
            viewmodel.VaccineID = null;
            //Inject mock data into the controller method
            mockVaccineShipmentRepo.Setup(m => m.ListAllVaccineShipments()).Returns(mockVaccineShipments);

            mockFacilityRepo.Setup(m => m.ListOfAllFacilities()).Returns(new List<Facility>());
            //2.Act (Actual)

            ViewResult result = controller.SearchVaccineShipments(viewmodel); //get back a result

            SearchVaccineShipmentsViewModel resultmodel = result.Model as SearchVaccineShipmentsViewModel;
            List<VaccineShipment> actualvaccineshipments = resultmodel.ResultListOfVaccineShipments;

            actualnumberofshipments = actualvaccineshipments.Count;
            //3. Assert (Fails->Succeeds)
            Assert.Equal(expectednumberofshipments, actualnumberofshipments);
        }

        [Fact]
        public void ShouldSearchVAccineShipmentsByStartAndEndDates()
        {
            //AAA Testing: We expect something to happen; Compare expected against Actual
            //1.Arrange

            int expectednumberofshipments = 2;
            int actualnumberofshipments = 0;
            List<VaccineShipment> mockVaccineShipments = CreateMockVaccineShipmentData();

            DateTime? startdate = new DateTime(2021, 4, 11);
            DateTime? enddate = new DateTime(2021, 4, 15);
            SearchVaccineShipmentsViewModel viewmodel = new SearchVaccineShipmentsViewModel();
            viewmodel.StartDate = startdate;
            viewmodel.EndDate = enddate;
            viewmodel.FacilityID = null;
            viewmodel.VaccineID = null;
            //Inject mock data into the controller method
            mockVaccineShipmentRepo.Setup(m => m.ListAllVaccineShipments()).Returns(mockVaccineShipments);

            mockFacilityRepo.Setup(m => m.ListOfAllFacilities()).Returns(new List<Facility>());
            //2.Act (Actual)

            ViewResult result = controller.SearchVaccineShipments(viewmodel) as ViewResult; //get back a result

            SearchVaccineShipmentsViewModel resultmodel = result.Model as SearchVaccineShipmentsViewModel;
            List<VaccineShipment> actualvaccineshipments = resultmodel.ResultListOfVaccineShipments;


            actualnumberofshipments = actualvaccineshipments.Count;
            //3. Assert (Fails->Succeeds)
            Assert.Equal(expectednumberofshipments, actualnumberofshipments);
        }
        [Fact]
        public void ShouldSearchVaccineShipmentsByStartAndEndDatesAndVaccine()
        {
            //AAA Testing: We expect something to happen; Compare expected against Actual
            //1.Arrange

            int expectednumberofshipments = 1;
            int actualnumberofshipments = 0;
            List<VaccineShipment> mockVaccineShipments = CreateMockVaccineShipmentData();

            DateTime? startdate = new DateTime(2021, 4, 11);
            DateTime? enddate = new DateTime(2021, 4, 15);
            int? vaccineid = 12;
            SearchVaccineShipmentsViewModel viewmodel = new SearchVaccineShipmentsViewModel();
            viewmodel.StartDate = startdate;
            viewmodel.EndDate = enddate;
            viewmodel.FacilityID = null;
            viewmodel.VaccineID = vaccineid;
            //Inject mock data into the controller method
            mockVaccineShipmentRepo.Setup(m => m.ListAllVaccineShipments()).Returns(mockVaccineShipments);

            mockFacilityRepo.Setup(m => m.ListOfAllFacilities()).Returns(new List<Facility>());
            //2.Act (Actual)

            ViewResult result = controller.SearchVaccineShipments(viewmodel) as ViewResult; //get back a result

            SearchVaccineShipmentsViewModel resultmodel = result.Model as SearchVaccineShipmentsViewModel;
            List<VaccineShipment> actualvaccineshipments = resultmodel.ResultListOfVaccineShipments;


            actualnumberofshipments = actualvaccineshipments.Count;
            //3. Assert (Fails->Succeeds)
            Assert.Equal(expectednumberofshipments, actualnumberofshipments);
        }
        [Fact]
        public void ShouldSearchVaccineShipmentsByStartAndEndDatesAndVaccineAndFacility()
        {
            int expectednumberofshipments = 1;
            int actualnumberofshipments = 0;
            List<VaccineShipment> mockVaccineShipments = CreateMockVaccineShipmentData();

            DateTime? startdate = new DateTime(2021, 4, 11);
            DateTime? enddate = new DateTime(2021, 4, 15);
            int? vaccineid = 12;
            int? facilityid = 2;
            SearchVaccineShipmentsViewModel viewmodel = new SearchVaccineShipmentsViewModel();
            viewmodel.StartDate = startdate;
            viewmodel.EndDate = enddate;
            viewmodel.FacilityID = facilityid;
            viewmodel.VaccineID = vaccineid;
            //Inject mock data into the controller method
            mockVaccineShipmentRepo.Setup(m => m.ListAllVaccineShipments()).Returns(mockVaccineShipments);

            mockFacilityRepo.Setup(m => m.ListOfAllFacilities()).Returns(new List<Facility>());
            //2.Act (Actual)

            ViewResult result = controller.SearchVaccineShipments(viewmodel) as ViewResult; //get back a result

            SearchVaccineShipmentsViewModel resultmodel = result.Model as SearchVaccineShipmentsViewModel;
            List<VaccineShipment> actualvaccineshipments = resultmodel.ResultListOfVaccineShipments;


            actualnumberofshipments = actualvaccineshipments.Count;
            //3. Assert (Fails->Succeeds)
            Assert.Equal(expectednumberofshipments, actualnumberofshipments);
        }
         
        [Fact]
        public void ShouldSearchVaccineShipmentsWithoutFiltering()
        {
            //AAA Testing: We expect something to happen; Compare expected against Actual
            //1.Arrange

            int expectednumberofshipments = 6;
            int actualnumberofshipments = 0;
            List<VaccineShipment> mockVaccineShipments = CreateMockVaccineShipmentData();

            DateTime? startdate = null;
            DateTime? enddate = null;
            int? vaccineid = null;
            SearchVaccineShipmentsViewModel viewmodel = new SearchVaccineShipmentsViewModel();
            viewmodel.StartDate = startdate;
            viewmodel.EndDate = enddate;
            viewmodel.FacilityID = null;
            viewmodel.VaccineID = vaccineid;
            //Inject mock data into the controller method
            mockVaccineShipmentRepo.Setup(m => m.ListAllVaccineShipments()).Returns(mockVaccineShipments);

            mockFacilityRepo.Setup(m => m.ListOfAllFacilities()).Returns(new List<Facility>());
            //2.Act (Actual)

            ViewResult result = controller.SearchVaccineShipments(viewmodel) as ViewResult; //get back a result

            SearchVaccineShipmentsViewModel resultmodel = result.Model as SearchVaccineShipmentsViewModel;
            List<VaccineShipment> actualvaccineshipments = resultmodel.ResultListOfVaccineShipments;


            actualnumberofshipments = actualvaccineshipments.Count;
            //3. Assert (Fails->Succeeds)
            Assert.Equal(expectednumberofshipments, actualnumberofshipments);

        }

        public List<VaccineShipment> CreateMockVaccineShipmentData()
        {
            List<VaccineShipment> mockVaccineShipmentsList = new List<VaccineShipment>();
            //IDE stands integrated development environment( VS, Eclipse)
            DateTime startdate = new DateTime(2021, 4, 4);//types of errors compile/build error and logic error

            VaccineShipment vaccineShipment = new VaccineShipment(1, 11, startdate, 15);
            mockVaccineShipmentsList.Add(vaccineShipment);

            vaccineShipment = new VaccineShipment(1, 12, startdate, 25);
            mockVaccineShipmentsList.Add(vaccineShipment);

            vaccineShipment = new VaccineShipment(1, 13, startdate, 35);
            mockVaccineShipmentsList.Add(vaccineShipment);

            startdate = new DateTime(2021, 4, 11);

            vaccineShipment = new VaccineShipment(2, 11, startdate, 101);
            mockVaccineShipmentsList.Add(vaccineShipment);

            DateTime enddate = new DateTime(2021, 4, 15);

            vaccineShipment = new VaccineShipment(2, 12, startdate, 151, enddate);
            mockVaccineShipmentsList.Add(vaccineShipment);

            vaccineShipment = new VaccineShipment(2, 13, startdate, 201, enddate);
            mockVaccineShipmentsList.Add(vaccineShipment);

            return mockVaccineShipmentsList;
        }

    }//end class
}//end of namespace
