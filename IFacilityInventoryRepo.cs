using CostelloClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Models
{
    public interface IFacilityInventoryRepo
    {
        //Refactoring : 1. Get it to work 2. Make it cleaner, more efficient
        //public void UpdateCurrentInventory(int facilityid, int vaccineid, int numberofvaccines);

        public void UpdateCurrentInventory(FacilityInventory facilityinventory);
        //int GetCurrentInventory(int facilityID, int vaccineID);

        FacilityInventory FindFacilityInventory(int facilityID, int vaccineID);
        FacilityInventory FindFacilityInventory(int facilityInventoryID);
    }
}
