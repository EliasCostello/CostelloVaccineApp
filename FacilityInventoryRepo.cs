using CostelloClassLibrary;
using CostelloMVCWebProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Models
{
    public class FacilityInventoryRepo : IFacilityInventoryRepo
    {
        private readonly ApplicationDbContext database;

        public FacilityInventoryRepo(ApplicationDbContext dbcontext)
        {
            this.database = dbcontext;

        }

        public FacilityInventory FindFacilityInventory(int facilityID, int vaccineID)
        {
            FacilityInventory facilityinventory = database.FacilityInventory.Where(f => f.FacilityID == facilityID && f.VaccineID == vaccineID).FirstOrDefault();

            return facilityinventory;
        }

        public FacilityInventory FindFacilityInventory(int facilityInventoryID)
        {
            FacilityInventory facilityinventory = database.FacilityInventory.Include(fi=>fi.Facility).Include(fi=>fi.Vaccine).Where(fi=>fi.FacilityInventoryID == facilityInventoryID).FirstOrDefault();
            return facilityinventory;
        }

        //public int GetCurrentInventory(int facilityID, int vaccineID)
        //{
        //    FacilityInventory facilityinventory = database.FacilityInventory.Where(f => f.FacilityID == facilityID && f.VaccineID == vaccineID).FirstOrDefault();
        //    int currentinventory = 0;
        //    if (facilityinventory != null)
        //    {
        //        currentinventory = facilityinventory.CurrentInventory;
        //    }
        //    return currentinventory;
        //}

        public void UpdateCurrentInventory(FacilityInventory facilityInventory)
        {
            database.FacilityInventory.Update(facilityInventory);
            database.SaveChanges();
        }
        //public void UpdateCurrentInventory(int facilityid, int vaccineid, int numberofvaccines)
        //{
        //    FacilityInventory facilityinventory = database.FacilityInventory.Where(f => f.FacilityID == facilityid && f.VaccineID == vaccineid).FirstOrDefault();
        //    facilityinventory.CurrentInventory += numberofvaccines;
        //    database.FacilityInventory.Update(facilityinventory);
        //    database.SaveChanges();

        //}
    }
}
