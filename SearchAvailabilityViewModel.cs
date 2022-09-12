using CostelloClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.ViewModels
{
    public class SearchAvailabilityViewModel
    {
        public int? VaccineID { get; set; }
        public int? FacilityID { get; set; }
        [DataType(DataType.Date)]
        public DateTime? AppointmentDate {get;set;}
        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }

        public List<AppointmentAvailability> ResultListOfAvailabilities { get; set; }

    }
}
