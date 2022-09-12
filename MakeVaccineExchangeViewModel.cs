using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.ViewModels
{
    public class MakeVaccineExchangeViewModel
    {
        [Required(ErrorMessage = "Exchanged Date is required")]
        [DataType(DataType.Date)]
        public DateTime ExchangedDate { get; set; }
        [Required(ErrorMessage = "Number of doses are required")]
        public int? ExchangedDoses { get; set; }
        [Required(ErrorMessage = "Vaccine ID is required")]
        public int VaccineID { get; set; }
        [Required(ErrorMessage = "Sending Facility ID is required")]
        public int SendingFacilityID { get; set; }
        [Required(ErrorMessage = "Receiving Facility ID is required")]
        public int ReceivingFacilityID { get; set; }
    }
}
