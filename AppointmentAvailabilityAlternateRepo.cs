using CostelloClassLibrary;
using CostelloMVCWebProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Models
{
    public class AppointmentAvailabilityAlternateRepo : IAppointmentAvailabilityAlternateRepo
    {
        private readonly ApplicationDbContext database;
        public AppointmentAvailabilityAlternateRepo(ApplicationDbContext dbContext)
        {
            this.database = dbContext;
        }

        public int CreateMultipleAppointmentScheduler(MultipleAppointmentScheduler multipleAppointmentScheduler)
        {
            database.MultipleAppointmentScheduler.Add(multipleAppointmentScheduler);
            database.SaveChanges();
            int schedulerID = multipleAppointmentScheduler.MultipleAppointmentSchedulerID;
            return schedulerID;
        }

        public AppointmentAvailabilityAlternate FindAppointmentAvailabilityAlternate(int id)
        {
            AppointmentAvailabilityAlternate appointmentAvailabilityAlternate =
                database.AppointmentAvailabilityAlternate.Find(id);
            return appointmentAvailabilityAlternate;
        }

        public List<AppointmentAvailabilityAlternate> ListAll()
        {
            return database.AppointmentAvailabilityAlternate.ToList();
        }

        public void UpdateAppointmentAvailabilityAlternate(AppointmentAvailabilityAlternate appointmentAvailabilityAlternate)
        {
            database.AppointmentAvailabilityAlternate.Update(appointmentAvailabilityAlternate);
            database.SaveChanges();
        }

    }//end class AppointmentAvailabilityAlternateRepontmentAvailabilityAlternateRepo
}
