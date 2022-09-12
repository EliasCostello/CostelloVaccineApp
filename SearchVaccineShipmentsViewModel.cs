using CostelloMVCWebProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CostelloClassLibrary;

namespace CostelloMVCWebProject.ViewModels
{
    public class SearchVaccineShipmentsViewModel
    {
        //For User Inputs
        public int? VaccineID { get; set; }
        public int? FacilityID { get; set; }
        [DataType(DataType.Date)] //datetime requires a time as well
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        //For Result
        public List<VaccineShipment> ResultListOfVaccineShipments {get;set;}
    }
}
