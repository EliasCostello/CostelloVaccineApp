using CostelloClassLibrary;
using CostelloMVCWebProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Models
{
    public class AppointmentAvailabilityRepo : IAppointmentAvailabilityRepo
    {
        private readonly ApplicationDbContext database;
        public AppointmentAvailabilityRepo(ApplicationDbContext applicationDbContext)
        {
            this.database = applicationDbContext;
        }

        public int AddAppointmentAvailability(AppointmentAvailability appointmentAvailability)
        {
            database.AppointmentAvailability.Add(appointmentAvailability);
            database.SaveChanges();
            return appointmentAvailability.AppointmentAvailabilityID;
        }

        public void EditAppointmentAvailability(AppointmentAvailability e)
        {
            database.AppointmentAvailability.Update(e);
            database.SaveChanges();
        }

        public AppointmentAvailability FindAppointmentAvailability(int appointmentavailabilityID)
        {
          AppointmentAvailability appavailablity = database.AppointmentAvailability.Include(aa=>aa.FacilityInventory.Facility).Include(aa=>aa.FacilityInventory).Where(aa=>aa.AppointmentAvailabilityID == appointmentavailabilityID).FirstOrDefault();
            return appavailablity;
        }

        public List<AppointmentAvailability> ListAllVaccinesAppointmentsForPatient()
        {
            //List<AppointmentAvailability> availabileappointments = database.AppointmentAvailability.Include(a => a.FacilityInventory).ThenInclude(fi=>fi.Facility).ToList();
            List<AppointmentAvailability> availabileappointments = database.AppointmentAvailability.Include(a => a.FacilityInventory.Facility).Include(a=>a.FacilityInventory.Vaccine).Include(a=>a.Patient).ToList();
            return availabileappointments;
        }

        public void ScheduleAppointment(AppointmentAvailability appointmentAvailability)
        {
            database.AppointmentAvailability.Update(appointmentAvailability);
            database.SaveChanges();
        }
    }
}
