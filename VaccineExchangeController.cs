using CostelloClassLibrary;
using CostelloMVCWebProject.Models;
using CostelloMVCWebProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Controllers
{
    public class VaccineExchangeController : Controller
    {
        private readonly IVaccineExchangeRepo ivaccineexchangerepo;
        private readonly IFacilityRepo ifacilityrepo;
        private readonly IFacilityInventoryRepo iFacilityInventoryRepo;

        public VaccineExchangeController(IVaccineExchangeRepo vaccineexchangerepo, IFacilityRepo facilityRepo, IFacilityInventoryRepo facilityInventoryRepo)
        {
            this.ivaccineexchangerepo = vaccineexchangerepo;
            this.ifacilityrepo = facilityRepo;
            this.iFacilityInventoryRepo = facilityInventoryRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListAllVaccineExchanges()
        {
            List<VaccineExchange> allexchanges = ivaccineexchangerepo.ListAllVaccineExchanges();
            return View(allexchanges);
        }
        [HttpGet]
        
        public IActionResult MakeVaccineExchange()
        {
            ViewData["AllFacilities"] = new SelectList(ifacilityrepo.ListOfAllFacilities(), "FacilityID", "FacilityName");//List of items, value(ID), text (facility name)

            return View();
        }
        [HttpPost]
        
        public IActionResult MakeVaccineExchange(MakeVaccineExchangeViewModel viewModel)
        {
            //string facilityadminid = iApplicationRepo.FindUserID();
            if(viewModel.SendingFacilityID== viewModel.ReceivingFacilityID)
            {
                ModelState.AddModelError("SameFacilityError", "A vaccine exchange cannot occur if the receiving and sending facility are the same. Please change one or both values for receving and sending facility");
            }

            FacilityInventory sendingfacilityinventory = iFacilityInventoryRepo.FindFacilityInventory(viewModel.SendingFacilityID, viewModel.VaccineID);

            int currentinventory = sendingfacilityinventory.CurrentInventory;

            //int currentinventory = iFacilityInventoryRepo.GetCurrentInventory(viewModel.SendingFacilityID, viewModel.VaccineID);
            if (viewModel.ExchangedDoses > currentinventory)
            {
                ModelState.AddModelError("InsufficientVaccinesError", "Number of Vaccines is more than is available at the Sending Facility");
            }
            if (ModelState.IsValid)
            {

                //ivaccineshipmentrepo.EditVaccineShipment(vaccineshipment)
                

                VaccineExchange vaccineexchange = new VaccineExchange(viewModel.ExchangedDate, viewModel.ExchangedDoses.Value, viewModel.SendingFacilityID, viewModel.ReceivingFacilityID, viewModel.VaccineID);
             
                int vaccineexchangeid = ivaccineexchangerepo.MakeVaccineExhange(vaccineexchange);

                vaccineexchange.VaccineExchangeID = vaccineexchangeid;
                //Update two places
                sendingfacilityinventory.CurrentInventory -= viewModel.ExchangedDoses.Value;
                iFacilityInventoryRepo.UpdateCurrentInventory(sendingfacilityinventory);

                FacilityInventory receivingfacilityinventory = iFacilityInventoryRepo.FindFacilityInventory(viewModel.ReceivingFacilityID, viewModel.VaccineID);

                receivingfacilityinventory.CurrentInventory += viewModel.ExchangedDoses.Value;

                iFacilityInventoryRepo.UpdateCurrentInventory(receivingfacilityinventory);

                return RedirectToAction("ListAllVaccineExchanges");
            }
            else
            {
                ViewData["AllFacilities"] = new SelectList(ifacilityrepo.ListOfAllFacilities(), "FacilityID", "FacilityName");

                return View(viewModel);
            }

        }
      
    }
}
