using CostelloClassLibrary;
using CostelloMVCWebProject.Models;
using CostelloMVCWebProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Controllers
{
    public class AppointmentSchedulerController : Controller
    {
        private readonly IFacilityRepo iFacilityRepo;
        private readonly IFacilityInventoryRepo iFacilityInventoryRepo;
        private readonly IAppointmentAvailabilityRepo iAppointmentInventoryRepo;
        private readonly IApplicationUserRepo iApplicationUserRepo;
        public AppointmentSchedulerController(IFacilityInventoryRepo facilityInventoryRepo, IAppointmentAvailabilityRepo appointmentAvailabilityRepo, IApplicationUserRepo applicationUserRepo, IFacilityRepo facilityRepo)
        {
            this.iFacilityInventoryRepo = facilityInventoryRepo;
            this.iAppointmentInventoryRepo = appointmentAvailabilityRepo;
            this.iApplicationUserRepo = applicationUserRepo;
            this.iFacilityRepo = facilityRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListAllVaccinesAppointmentsForPatient()
        {
            List<AppointmentAvailability> allavailabilities = iAppointmentInventoryRepo.ListAllVaccinesAppointmentsForPatient();

            string patientID = iApplicationUserRepo.FindUserID();

            allavailabilities = allavailabilities.Where(aa => aa.PatientId == patientID).ToList();
            return View(allavailabilities);
        }
        public IActionResult ScheduleAppointment(int appointmentavailabilityID)
        {
            string patientID = iApplicationUserRepo.FindUserID();
            AppointmentAvailability appointmentAvailability = iAppointmentInventoryRepo.FindAppointmentAvailability(appointmentavailabilityID);
            FacilityInventory facilityinventory = iFacilityInventoryRepo.FindFacilityInventory(appointmentAvailability.FacilityInventoryID);

            int currentinventory = facilityinventory.CurrentInventory;
            if (currentinventory == 0)
            {
                ModelState.AddModelError("NoInventory", "Sorry there are no more vaccines available at this location" + "Facility: " + facilityinventory.Facility.FacilityName + "Vaccine: " + facilityinventory.Vaccine.VaccineName);
            }

            if (appointmentAvailability.AppointmentStatus == AppointmentStatusOptions.Booked)
            {
                ModelState.AddModelError("AlreadyBooked", "Sorry that time slot has been booked already." + "Date: " + appointmentAvailability.AppointmentDateStartTime.ToShortDateString() + "Time: " + appointmentAvailability.AppointmentDateStartTime.ToShortTimeString());
            }

            if (ModelState.IsValid)
            {
                appointmentAvailability.PatientId = patientID;
                appointmentAvailability.AppointmentBooked = DateTime.Now;
                appointmentAvailability.AppointmentStatus = AppointmentStatusOptions.Booked;

                iAppointmentInventoryRepo.ScheduleAppointment(appointmentAvailability);
                facilityinventory.CurrentInventory -= 1;
                iFacilityInventoryRepo.UpdateCurrentInventory(facilityinventory);

                return RedirectToAction("ListAllVaccinesAppointmentsForPatient");
            }
            else
            {
                ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");

                SearchAvailabilityViewModel viewmodel = new SearchAvailabilityViewModel();
                viewmodel.ResultListOfAvailabilities = null;

                return View("SearchForAppointmentAvailability", viewmodel);
            }

        }

 

        [Authorize(Roles="Patient")]
        [HttpGet]
        public ViewResult SearchForAppointmentAvailability()
        {
            //dynamic dropdown
            ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");

            SearchAvailabilityViewModel viewmodel = new SearchAvailabilityViewModel();

            viewmodel.ResultListOfAvailabilities = null;

            return View(viewmodel);
        }
        [Authorize(Roles = "Patient")]
        [HttpPost]
        public ViewResult SearchForAppointmentAvailability(SearchAvailabilityViewModel viewmodel)//required input (not optional)
        {
            List<AppointmentAvailability> allavailabilities = iAppointmentInventoryRepo.ListAllVaccinesAppointmentsForPatient();

            allavailabilities = allavailabilities.Where(a => a.AppointmentStatus == AppointmentStatusOptions.Open && a.FacilityInventory.CurrentInventory >0).ToList();

            allavailabilities = allavailabilities.OrderBy(a => a.AppointmentDateStartTime).ToList();

            if (viewmodel.AppointmentDate != null)
            {
                allavailabilities = allavailabilities.Where(a => a.AppointmentDateStartTime.Date >= viewmodel.AppointmentDate.Value.Date).ToList();
            }
            if (viewmodel.StartTime != null)
            {
                allavailabilities = allavailabilities.Where(a => a.AppointmentDateStartTime.TimeOfDay >= viewmodel.StartTime.Value.TimeOfDay).ToList();
            }
            if (viewmodel.EndTime != null)
            {
                allavailabilities = allavailabilities.Where(a => a.AppointmentDateEndTime.TimeOfDay <= viewmodel.EndTime.Value.TimeOfDay).ToList();
            }
            if (viewmodel.VaccineID != null)
            {
                allavailabilities = allavailabilities.Where(a => a.FacilityInventory.VaccineID == viewmodel.VaccineID).ToList();
            }
            if (viewmodel.FacilityID != null)
            {
                allavailabilities = allavailabilities.Where(a => a.FacilityInventory.FacilityID == viewmodel.FacilityID).ToList();
            }
            viewmodel.ResultListOfAvailabilities = allavailabilities.Where(a=>a.AppointmentStatus == AppointmentStatusOptions.Open && a.FacilityInventory.CurrentInventory>0).ToList();

            ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");
            return View("SearchForAppointmentAvailability", viewmodel);
        }

        public IActionResult ConfirmCancelAppointment(int appointmentAvailabilityID)
        {

            ViewData["AllFacilities"] =
                            new SelectList(iFacilityRepo.ListOfAllFacilities(),
                            "FacilityID", "FacilityName");

            AppointmentAvailability appointmentAvailability = iAppointmentInventoryRepo.FindAppointmentAvailability(appointmentAvailabilityID);
            return View(appointmentAvailability);
        }
        [HttpGet]
        [Authorize(Roles = "FacilityAdmin")]
        public IActionResult AddNewAppointmentAvailability()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "FacilityAdmin")]
        public IActionResult AddNewAppointmentAvailability(AddAppointmentAvailabilityViewModel viewmodel)
        {
            FacilityInventory facilityinventory = iFacilityInventoryRepo.FindFacilityInventory(viewmodel.FacilityInventoryID);
            int currentinventory = facilityinventory.CurrentInventory;

            if (currentinventory == 0)
            {
                ModelState.AddModelError("NoInventory", "There is no inventory of the selected vaccine at this location");
            }
            if (ModelState.IsValid)
            {
                AppointmentAvailability appointmentAvailability = new AppointmentAvailability(viewmodel.FacilityInventoryID, viewmodel.AppointmentStartTime);

                int appointmentavailabilityID = iAppointmentInventoryRepo.AddAppointmentAvailability(appointmentAvailability);

                appointmentAvailability.AppointmentAvailabilityID = appointmentavailabilityID;

                return RedirectToAction("ListAllVaccinesAppointmentsForPatient");
            }
            else
            {
                return View(viewmodel);
            }
        }
        [HttpGet]
        [Authorize(Roles = "FacilityAdmin")]
        public IActionResult EditAppointment(int appointmentavailibilityid)
        {
            AppointmentAvailability appointmentavailability = iAppointmentInventoryRepo.FindAppointmentAvailability(appointmentavailibilityid);
            return View(appointmentavailability);
        }

        [HttpPost]
        [Authorize(Roles = "FacilityAdmin")]
        public IActionResult EditAppointment(AppointmentAvailability appointmentAvailability)
        {
            if (ModelState.IsValid)
            {
                iAppointmentInventoryRepo.EditAppointmentAvailability(appointmentAvailability);
                return RedirectToAction("ListAllVaccinesAppointmentsForPatient");
            }
            else
            {
                return View(appointmentAvailability);
            }

        }
        public IActionResult CancelAppointment(string btnSubmit, AppointmentAvailability appointmentAvailabilityCancelled)
        {
            if (btnSubmit == "Confirm")
            {


                FacilityInventory facilityInventory = iFacilityInventoryRepo.FindFacilityInventory(appointmentAvailabilityCancelled.FacilityInventoryID);

                if (appointmentAvailabilityCancelled.AppointmentStatus != AppointmentStatusOptions.Booked)
                {
                    ModelState.AddModelError("AppointmentStatusError", "The apppointment which is on:" + appointmentAvailabilityCancelled.AppointmentDateStartTime + "cannot be cancelled. Its status is " + appointmentAvailabilityCancelled.AppointmentStatus);
                }
               
                    appointmentAvailabilityCancelled.AppointmentStatus = AppointmentStatusOptions.Canceled;
                    iAppointmentInventoryRepo.EditAppointmentAvailability(appointmentAvailabilityCancelled);

                    facilityInventory.CurrentInventory += 1;
                    iFacilityInventoryRepo.UpdateCurrentInventory(facilityInventory);

                    AppointmentAvailability freshappointmentavailability = new AppointmentAvailability(appointmentAvailabilityCancelled.FacilityInventoryID, appointmentAvailabilityCancelled.AppointmentDateStartTime);
                    freshappointmentavailability.AppointmentStatus = AppointmentStatusOptions.Open;
                    iAppointmentInventoryRepo.AddAppointmentAvailability(freshappointmentavailability);


                
                return RedirectToAction("ListAllVaccinesAppointmentsForPatient");
            }
            else
            {
                return RedirectToAction("ListAllVaccinesAppointmentsForPatient");
            }
        }

    }
}
