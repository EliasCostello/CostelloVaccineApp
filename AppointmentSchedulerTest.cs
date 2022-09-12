using CostelloClassLibrary;
using CostelloMVCWebProject.Controllers;
using CostelloMVCWebProject.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CostelloTestProject
{
    public class AppointmentSchedulerTest
    {
        private readonly Mock<IFacilityRepo> mockFacilityRepo;
        private readonly Mock<IFacilityInventoryRepo> mockFacilityInventoryRepo;
        private readonly Mock<IAppointmentAvailabilityRepo> mockAppointmentAvailibilityRepo;
        private readonly Mock<IApplicationUserRepo> mockApplicationUserRepo;

        private readonly AppointmentSchedulerController controller;

        public AppointmentSchedulerTest()
        {
            mockFacilityInventoryRepo = new Mock<IFacilityInventoryRepo>();
            mockAppointmentAvailibilityRepo = new Mock<IAppointmentAvailabilityRepo>();
            mockApplicationUserRepo = new Mock<IApplicationUserRepo>();
            mockFacilityRepo = new Mock<IFacilityRepo>();
            controller = new AppointmentSchedulerController(mockFacilityInventoryRepo.Object, mockAppointmentAvailibilityRepo.Object, mockApplicationUserRepo.Object, mockFacilityRepo.Object);

        }
        [Fact]
        public void ShouldScheduleAppointment()
        {
            //Arrange
          
            int appointmentAvailibilityID = 1;
            int facilityinventoryid = 11;
            DateTime appstarttime = DateTime.Now.AddDays(7);
            AppointmentAvailability appointmentavailability = new AppointmentAvailability(facilityinventoryid, appstarttime);
            appointmentavailability.AppointmentAvailabilityID = appointmentAvailibilityID;
            mockAppointmentAvailibilityRepo.Setup(m => m.FindAppointmentAvailability(appointmentAvailibilityID)).Returns(appointmentavailability);
            int facilityid = 101; int vaccineid = 201;
            mockFacilityRepo.Setup(m => m.ListOfAllFacilities()).Returns(new List<Facility>());
            FacilityInventory facilityInventory = new FacilityInventory(facilityid, vaccineid);
            facilityInventory.CurrentInventory = 1;

            mockFacilityInventoryRepo.Setup(m => m.FindFacilityInventory(facilityinventoryid)).Returns(facilityInventory);
            string patientid = "P10001";
            mockApplicationUserRepo.Setup(m => m.FindUserID()).Returns(patientid);

            mockAppointmentAvailibilityRepo.Setup(m => m.ScheduleAppointment(It.IsAny<AppointmentAvailability>()));

            mockFacilityInventoryRepo.Setup(m => m.UpdateCurrentInventory(It.IsAny<FacilityInventory>()));
            //Act
            
            controller.ScheduleAppointment(appointmentAvailibilityID);
            //Assert
            mockAppointmentAvailibilityRepo.Verify(m => m.ScheduleAppointment(It.IsAny<AppointmentAvailability>()), Times.Exactly(1));

            mockFacilityInventoryRepo.Verify(m => m.UpdateCurrentInventory(It.IsAny<FacilityInventory>()), Times.Exactly(1));
            int expectedcurrentinventory = 0;
            Assert.Equal(expectedcurrentinventory, facilityInventory.CurrentInventory);
            Assert.Equal(AppointmentStatusOptions.Booked, appointmentavailability.AppointmentStatus);
            
        }
    }
}
