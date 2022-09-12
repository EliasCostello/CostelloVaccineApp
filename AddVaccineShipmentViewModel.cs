using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.ViewModels
{
    public class AddVaccineShipmentViewModel
    {
        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        [Required]
        public int? NumberOfVaccinesShipped { get; set; }
        public int FacilityID { get; set; }

        public int VaccineID { get; set; }
    }
}
