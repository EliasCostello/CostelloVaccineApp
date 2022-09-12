using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.ViewModels
{
    public class AddAppointmentAvailabilityViewModel
    {
        [Required(ErrorMessage = "FacilityInventoryID is required")]
        public int FacilityInventoryID;
        [Required(ErrorMessage = "AppointmentStartTime is required")]
        public DateTime AppointmentStartTime;
        
    }
}
