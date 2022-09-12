using CostelloClassLibrary;
using CostelloMVCWebProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Controllers
{
    public class MultipleAppointmentSchedulerController : Controller
    {
        private readonly IAppointmentAvailabilityAlternateRepo iAppointmentAvailabilityAlternateRepo;
        private readonly IApplicationUserRepo iApplicationUserRepo;
        public MultipleAppointmentSchedulerController(IAppointmentAvailabilityAlternateRepo
        appointmentAvailabilityAlternateRepo, IApplicationUserRepo applicationUserRepo)
        {
            this.iAppointmentAvailabilityAlternateRepo = appointmentAvailabilityAlternateRepo;
            this.iApplicationUserRepo = applicationUserRepo;
        }
        public IActionResult ListAllAppointmentAvailabilitiesAlternate()
        {
            List<AppointmentAvailabilityAlternate> listAll =
            iAppointmentAvailabilityAlternateRepo.ListAll();
            return View(listAll);
        }
        public IActionResult ScheduleMultipleAppointments(int appointmentAvailabilityAlternateID)
        {
            if (HttpContext.Session.GetInt32("schedulerID") == null)
            {
                string patientID = iApplicationUserRepo.FindUserID();
                MultipleAppointmentScheduler multipleAppointmentScheduler =
                new MultipleAppointmentScheduler(patientID);
                int schedulerID = iAppointmentAvailabilityAlternateRepo
                .CreateMultipleAppointmentScheduler(multipleAppointmentScheduler);
                HttpContext.Session.SetInt32("schedulerID", schedulerID);
                AppointmentAvailabilityAlternate appointment =
                iAppointmentAvailabilityAlternateRepo.FindAppointmentAvailabilityAlternate(appointmentAvailabilityAlternateID);
                appointment.MultipleAppointmentSchedulerID = schedulerID;
                appointment.AppointmentStatus = AppointmentStatusOptions.Booked;
                iAppointmentAvailabilityAlternateRepo.UpdateAppointmentAvailabilityAlternate(appointment);
            }
            else
            {
                AppointmentAvailabilityAlternate appointment =
                iAppointmentAvailabilityAlternateRepo.FindAppointmentAvailabilityAlternate(appointmentAvailabilityAlternateID);
                appointment.MultipleAppointmentSchedulerID =
                HttpContext.Session.GetInt32("schedulerID");
                appointment.AppointmentStatus = AppointmentStatusOptions.Booked;
                iAppointmentAvailabilityAlternateRepo.UpdateAppointmentAvailabilityAlternate(appointment);
            }
            return RedirectToAction("ListAllAppointmentAvailabilitiesAlternate");
        }
    }
}
