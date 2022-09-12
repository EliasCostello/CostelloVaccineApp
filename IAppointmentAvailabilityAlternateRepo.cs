using CostelloClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Models
{
    public interface IAppointmentAvailabilityAlternateRepo
    {
        List<AppointmentAvailabilityAlternate> ListAll();
        int CreateMultipleAppointmentScheduler(MultipleAppointmentScheduler multipleAppointmentScheduler);
        AppointmentAvailabilityAlternate FindAppointmentAvailabilityAlternate(int id);
        void UpdateAppointmentAvailabilityAlternate(AppointmentAvailabilityAlternate appointmentAvailabilityAlternate);
    }
}
