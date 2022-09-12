using CostelloClassLibrary;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CostelloMVCWebProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<AppointmentAvailabilityAlternate> AppointmentAvailabilityAlternate { get; set; }
        public DbSet<MultipleAppointmentScheduler> MultipleAppointmentScheduler { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Facility> Facility { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Vaccine> Vaccine { get; set; }
        public DbSet<VaccineShipment> VaccineShipment { get; set; }
        public DbSet<FacilityAdmin> FacilityAdmin { get; set; }
        public DbSet<VaccineExchange> VaccineExchange { get; set; }

        public DbSet<FacilityInventory> FacilityInventory { get; set; }
        public DbSet<AppointmentAvailability> AppointmentAvailability { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
    }
}
