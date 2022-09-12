using CostelloMVCWebProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostelloClassLibrary;

namespace CostelloMVCWebProject.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider services)
        {
            //Initialization of data (sample data)
            //1. database
            ApplicationDbContext database = services.GetRequiredService<ApplicationDbContext>();

            //2. Roles
            RoleManager<IdentityRole> rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();

            //3. User
            UserManager<ApplicationUser> usermanager = services.GetRequiredService<UserManager<ApplicationUser>>();

            string hubmanagerrole = "HubManager";
            string facilityadminrole = "FacilityAdmin";
            string patientrole = "Patient";
            string sysadminrole = "SystemAdmin";
            if(!database.Roles.Any())
            {
                IdentityRole role = new IdentityRole(hubmanagerrole);
                rolemanager.CreateAsync(role).Wait();

                role = new IdentityRole(facilityadminrole);
                rolemanager.CreateAsync(role).Wait();

                role = new IdentityRole(patientrole);
                rolemanager.CreateAsync(role).Wait();

                role = new IdentityRole(sysadminrole);
                rolemanager.CreateAsync(role).Wait();
            }
               
            if(!database.ApplicationUser.Any())
            {
                DateTime dob = new DateTime(2000, 10, 1);
                Patient patient = new Patient("Test", "Patient", "Test.Patient@test.com", "3040001234", "Test.Patient", "109 Wilson Ave, Morgantown, WV 26501", dob, 3);
                patient.EmailConfirmed = true;
                usermanager.CreateAsync(patient).Wait();
                usermanager.AddToRoleAsync(patient, patientrole).Wait();

                patient = new Patient("Test", "Patient", "Test.Patient1@test.com", "3040001234", "Test.Patient1", "109 Wilson Ave, Morgantown, WV 26501", dob, 3);
                patient.EmailConfirmed = true;
                usermanager.CreateAsync(patient).Wait();
                usermanager.AddToRoleAsync(patient, patientrole).Wait();

                ApplicationUser applicationuser = new ApplicationUser("Test", "SystemAdmin", "Test.SysAdmin@test.com", "3046391234", "Test.SysAdmin");
                applicationuser.EmailConfirmed = true;
                usermanager.CreateAsync(applicationuser).Wait();
                usermanager.AddToRoleAsync(applicationuser, sysadminrole).Wait();

                FacilityAdmin facilityadmin = new FacilityAdmin("Test", "HospitalFacilityAdmin", "Test.HospitalFacilityAdmin@test.com", "3040001235", "Test.HospitalFacilityAdmin");
                facilityadmin.EmailConfirmed = true;
                usermanager.CreateAsync(facilityadmin).Wait();
                usermanager.AddToRoleAsync(facilityadmin, facilityadminrole).Wait();

                facilityadmin = new FacilityAdmin("Test", "PharmacyFacilityAdmin", "Test.PharmacyFacilityAdmin@test.com", "3040001236", "Test.PharmacyFacilityAdmin");
                facilityadmin.EmailConfirmed = true;
                usermanager.CreateAsync(facilityadmin).Wait();
                usermanager.AddToRoleAsync(facilityadmin, facilityadminrole).Wait();

                facilityadmin = new FacilityAdmin("Test", "NursingHomeFacilityAdmin", "Test.NursingHomeFacilityAdmin@test.com", "3040001237", "Test.NursingHomeFacilityAdmin");
                facilityadmin.EmailConfirmed = true;
                usermanager.CreateAsync(facilityadmin).Wait();
                usermanager.AddToRoleAsync(facilityadmin, facilityadminrole).Wait();


            }


            if(!database.Vaccine.Any())
            {
                Vaccine vaccine = new Vaccine("Pzifer", 3, 2);
                database.Vaccine.Add(vaccine);
                database.SaveChanges();

                vaccine = new Vaccine("Moderna", 4, 2);
                database.Vaccine.Add(vaccine);
                database.SaveChanges();

                vaccine = new Vaccine("Johnson & Johnson", 0, 1);
                database.Vaccine.Add(vaccine);
                database.SaveChanges();
            }

            if(!database.Facility.Any())
            {
                Facility facility = new Facility("WVU Hospital", "1 Medical Center Dr, Morgantown, WV 26506");
                database.Facility.Add(facility);
                database.SaveChanges();
                FacilityAdmin facilityAdminForFacility =
                 database.FacilityAdmin.Where(n => n.Email == "Test.HospitalFacilityAdmin@test.com").FirstOrDefault();

                facility.FacilityAdminID = facilityAdminForFacility.Id;
                database.Facility.Update(facility);
                database.SaveChanges();

                facilityAdminForFacility.FacilityID = 1;
                
                facility = new Facility("Walgreens", "897 Chestnut Ridge Rd, Morgantown, WV 26505");
                database.Facility.Add(facility);
                database.SaveChanges();

                facilityAdminForFacility =
                 database.FacilityAdmin.Where(n => n.Email == "Test.PharmacyFacilityAdmin@test.com").FirstOrDefault();

                facility.FacilityAdminID = facilityAdminForFacility.Id;
                database.Facility.Update(facility);
                database.SaveChanges();

                facilityAdminForFacility.FacilityID = 2;

                facility = new Facility("Sundale Nursing Home", "800 J D Anderson Dr, Morgantown, WV 26505");
                database.Facility.Add(facility);
                database.SaveChanges();

                facilityAdminForFacility =
                 database.FacilityAdmin.Where(n => n.Email == "Test.NursingHomeFacilityAdmin@test.com").FirstOrDefault();

                facility.FacilityAdminID = facilityAdminForFacility.Id;
                database.Facility.Update(facility);
                database.SaveChanges();

                facilityAdminForFacility.FacilityID = 3;

            }

            if(!database.VaccineShipment.Any())
            {
                List<VaccineShipment> vaccineShipmentsList = new List<VaccineShipment>();
               
                DateTime startdate = new DateTime(2021, 4, 4);

                VaccineShipment vaccineShipment = new VaccineShipment(1, 1, startdate, 15);
                vaccineShipmentsList.Add(vaccineShipment);

                vaccineShipment = new VaccineShipment(1, 2, startdate, 25);
                vaccineShipmentsList.Add(vaccineShipment);

                vaccineShipment = new VaccineShipment(1, 3, startdate, 35);
                vaccineShipmentsList.Add(vaccineShipment);

                startdate = new DateTime(2021, 4, 11);

                vaccineShipment = new VaccineShipment(2, 1, startdate, 101);
                vaccineShipmentsList.Add(vaccineShipment);

                DateTime enddate = new DateTime(2021, 4, 15);

                vaccineShipment = new VaccineShipment(2, 2, startdate, 151, enddate);
                vaccineShipmentsList.Add(vaccineShipment);

                vaccineShipment = new VaccineShipment(2, 3, startdate, 201, enddate);
                vaccineShipmentsList.Add(vaccineShipment);

                database.VaccineShipment.AddRange(vaccineShipmentsList);
                database.SaveChanges();
                
            }

            if(!database.FacilityInventory.Any())
            {
                FacilityInventory facilityInventory = new FacilityInventory(1, 1);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();

                facilityInventory = new FacilityInventory(1, 2);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();

                facilityInventory = new FacilityInventory(1, 3);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();

                facilityInventory = new FacilityInventory(2, 1);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();

                facilityInventory = new FacilityInventory(2, 2);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();

                facilityInventory = new FacilityInventory(2, 3);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();

                facilityInventory = new FacilityInventory(3, 1);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();

                facilityInventory = new FacilityInventory(3, 2);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();

                facilityInventory = new FacilityInventory(3, 3);
                facilityInventory.CurrentInventory = 100;
                database.FacilityInventory.Add(facilityInventory);
                database.SaveChanges();
            }
            if (!database.AppointmentAvailability.Any())
            {
                DateTime availabilityDateTime = DateTime.Now.AddDays(7);
                AppointmentAvailability appointmentAvailability = new AppointmentAvailability(1,availabilityDateTime);


                Patient patient = database.Patient.Where(p => p.Email == "Test.Patient1@test.com").FirstOrDefault();
                string patientID = patient.Id;
                appointmentAvailability.PatientId = patientID;
                appointmentAvailability.AppointmentBooked = DateTime.Now;
                appointmentAvailability.AppointmentStatus = AppointmentStatusOptions.Booked;

                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();

                FacilityInventory facilityInventory = database.FacilityInventory.Find(1);
                facilityInventory.CurrentInventory -= 1;
                database.FacilityInventory.Update(facilityInventory);
                database.SaveChanges();


                appointmentAvailability = new AppointmentAvailability(2, availabilityDateTime);
                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();

                appointmentAvailability = new AppointmentAvailability(3, availabilityDateTime);
                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();

                appointmentAvailability = new AppointmentAvailability(4, availabilityDateTime);
                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();

                appointmentAvailability = new AppointmentAvailability(5, availabilityDateTime);
                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();

                appointmentAvailability = new AppointmentAvailability(6, availabilityDateTime);
                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();

                appointmentAvailability = new AppointmentAvailability(7, availabilityDateTime);
                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();

                appointmentAvailability = new AppointmentAvailability(8, availabilityDateTime);
                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();

                appointmentAvailability = new AppointmentAvailability(9, availabilityDateTime);
                database.AppointmentAvailability.Add(appointmentAvailability);
                database.SaveChanges();
            }

            if (!database.AppointmentAvailabilityAlternate.Any())
            {
                DateTime availabilityDateTime = DateTime.Now.AddDays(7);

                AppointmentAvailabilityAlternate appointmentAvailabilityAlternate = new AppointmentAvailabilityAlternate(availabilityDateTime);
                database.AppointmentAvailabilityAlternate.Add(appointmentAvailabilityAlternate);
                database.SaveChanges();

                availabilityDateTime = availabilityDateTime.AddMinutes(30);

                appointmentAvailabilityAlternate = new AppointmentAvailabilityAlternate(availabilityDateTime);
                database.AppointmentAvailabilityAlternate.Add(appointmentAvailabilityAlternate);
                database.SaveChanges();

                availabilityDateTime = availabilityDateTime.AddMinutes(30);

                appointmentAvailabilityAlternate = new AppointmentAvailabilityAlternate(availabilityDateTime);
                database.AppointmentAvailabilityAlternate.Add(appointmentAvailabilityAlternate);
                database.SaveChanges();

                availabilityDateTime = availabilityDateTime.AddDays(7);

                appointmentAvailabilityAlternate = new AppointmentAvailabilityAlternate(availabilityDateTime);
                database.AppointmentAvailabilityAlternate.Add(appointmentAvailabilityAlternate);
                database.SaveChanges();

                availabilityDateTime = availabilityDateTime.AddMinutes(30);

                appointmentAvailabilityAlternate = new AppointmentAvailabilityAlternate(availabilityDateTime);
                database.AppointmentAvailabilityAlternate.Add(appointmentAvailabilityAlternate);
                database.SaveChanges();

                availabilityDateTime = availabilityDateTime.AddMinutes(30);

                appointmentAvailabilityAlternate = new AppointmentAvailabilityAlternate(availabilityDateTime);
                database.AppointmentAvailabilityAlternate.Add(appointmentAvailabilityAlternate);
                database.SaveChanges();
            }

        }

    }
}
