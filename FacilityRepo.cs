using CostelloMVCWebProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostelloClassLibrary;
namespace CostelloMVCWebProject.Models
{
    public class FacilityRepo : IFacilityRepo
    {
        private readonly ApplicationDbContext database;

        public FacilityRepo(ApplicationDbContext dbcontext)
        {
            this.database = dbcontext;

        }

        public Facility FindFacility(int? facilityID)
        {
            Facility facility = database.Facility.Find(facilityID);
            return facility;
        }

        public List<Facility> ListOfAllFacilities()
        {
           return   database.Facility.ToList();
        }
    }
}
