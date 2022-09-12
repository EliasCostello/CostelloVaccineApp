using CostelloMVCWebProject.Models;
using CostelloClassLibrary;
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
    
    public class VaccineShipmentController : Controller
    {
        private readonly IVaccineShipmentRepo ivaccineshipmentrepo;
        private readonly IFacilityRepo iFacilityRepo;
        private readonly IApplicationUserRepo iApplicationRepo;
        private readonly IFacilityInventoryRepo iFacilityInventoryRepo;

        public VaccineShipmentController(IVaccineShipmentRepo vaccineShipmentRepo, IFacilityRepo facilityrepo, IApplicationUserRepo applicationUserRepo, IFacilityInventoryRepo facilityinventoryrepo)
        {
            this.ivaccineshipmentrepo = vaccineShipmentRepo;
            this.iFacilityRepo = facilityrepo;
            this.iApplicationRepo = applicationUserRepo;
            this.iFacilityInventoryRepo = facilityinventoryrepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "FacilityAdmin")]
        public IActionResult ListAllVaccineShipments()
        {
            List<VaccineShipment> allvaccineshipment = ivaccineshipmentrepo.ListAllVaccineShipments();
            return View(allvaccineshipment);
        }

        [HttpGet]
        public IActionResult EditVaccineShipment(int? vaccineShipmentID)
        {
            ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");//List of items, value(ID), text (facility name)


            VaccineShipment vaccineshipment = ivaccineshipmentrepo.FindVaccineShipment(vaccineShipmentID);

            return View(vaccineshipment);
        }
        [HttpPost]
        public IActionResult EditVaccineShipment(VaccineShipment vaccineshipment)
        {
            if (ModelState.IsValid)
            {
                ivaccineshipmentrepo.EditVaccineShipment(vaccineshipment);
                return RedirectToAction("ListAllVaccineShipments");
                //VaccineShipment vaccineshipment = new VaccineShipment(viewmodel.FacilityID, viewmodel.VaccineID, viewmodel.StartDate.Value, viewmodel.NumberOfVaccinesShipped.Value, viewmodel.EndDate);

                //int vaccineshipmentid = ivaccineshipmentrepo.AddVaccineShipment(vaccineshipment);

                ////vaccineshipment.VaccineShipmentID = vaccineshipmentid;

                //return RedirectToAction("ListAllVaccineShipments");
            }
            else
            {
                ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");

                return View(vaccineshipment);
            }

        }
        
        [HttpGet]
        [Authorize(Roles = "FacilityAdmin")]
        public IActionResult AddVaccineShipment()
        {
            ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");//List of items, value(ID), text (facility name)

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "FacilityAdmin")]
        public IActionResult AddVaccineShipment(AddVaccineShipmentViewModel viewmodel)
        {
            string facilityadminid = iApplicationRepo.FindUserID();
            if (viewmodel.VaccineID>2940328)
            {
                ModelState.AddModelError("VaccineIncorrectError", "The vaccine ID you entered is invalid yo!");
            }
            if (ModelState.IsValid)
            {
                
                //ivaccineshipmentrepo.EditVaccineShipment(vaccineshipment)
                //return RedirectToAction("ListAllVaccineShipments");

                VaccineShipment vaccineshipment = new VaccineShipment(viewmodel.FacilityID, viewmodel.VaccineID, viewmodel.StartDate.Value, viewmodel.NumberOfVaccinesShipped.Value, viewmodel.EndDate);
                vaccineshipment.FacilityAdminID = facilityadminid;
                int vaccineshipmentid = ivaccineshipmentrepo.AddVaccineShipment(vaccineshipment);

                FacilityInventory facilityinventory = iFacilityInventoryRepo.FindFacilityInventory(viewmodel.FacilityID, viewmodel.VaccineID);
                facilityinventory.CurrentInventory += viewmodel.NumberOfVaccinesShipped.Value;
                iFacilityInventoryRepo.UpdateCurrentInventory(facilityinventory);

                //vaccineshipment.VaccineShipmentID = vaccineshipmentid;
              
                return RedirectToAction("ListAllVaccineShipments");
            }
            else
            {
                ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");

                return View(viewmodel);
            }
            
        }


        [HttpGet]
        public ViewResult SearchVaccineShipments()
        {
            //dynamic dropdown
            //list of all facilities
            ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");//List of items, value(ID), text (facility name)

            SearchVaccineShipmentsViewModel viewmodel = new SearchVaccineShipmentsViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        public ViewResult SearchVaccineShipments(SearchVaccineShipmentsViewModel viewmodel)//required input (not optional)
        {
            List<VaccineShipment> allvaccineshipment = ivaccineshipmentrepo.ListAllVaccineShipments();
            if (viewmodel.StartDate != null)
            {
                allvaccineshipment = allvaccineshipment.Where(a => a.StartDate >= viewmodel.StartDate).ToList();
            }
            if (viewmodel.EndDate != null)
            {
                allvaccineshipment = allvaccineshipment.Where(a => a.EndDate <= viewmodel.EndDate).ToList();
            }
            if (viewmodel.VaccineID != null)
            {
                allvaccineshipment = allvaccineshipment.Where(a => a.VaccineID == viewmodel.VaccineID).ToList();
            }
            if (viewmodel.FacilityID != null)
            {
                allvaccineshipment = allvaccineshipment.Where(a => a.FacilityID == viewmodel.FacilityID).ToList();
            }
            viewmodel.ResultListOfVaccineShipments = allvaccineshipment;

            ViewData["AllFacilities"] = new SelectList(iFacilityRepo.ListOfAllFacilities(), "FacilityID", "FacilityName");
            return View(viewmodel);
        }
       
    } //end class
}// end of namespace
