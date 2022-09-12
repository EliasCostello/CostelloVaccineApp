using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostelloClassLibrary;

namespace CostelloMVCWebProject.Models
{
    public interface IFacilityRepo
    {
        List<Facility> ListOfAllFacilities();
        Facility FindFacility(int? facilityID);
    }
}
