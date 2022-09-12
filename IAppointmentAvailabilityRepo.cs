using CostelloClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Models
{
    public interface IAppointmentAvailabilityRepo
    {
        AppointmentAvailability FindAppointmentAvailability(int appointmentavailabilityID);
        void ScheduleAppointment(AppointmentAvailability appointmentAvailability);
        List<AppointmentAvailability> ListAllVaccinesAppointmentsForPatient();
        
        void EditAppointmentAvailability(AppointmentAvailability e);
        int AddAppointmentAvailability(AppointmentAvailability appointmentAvailability);
    }
}
