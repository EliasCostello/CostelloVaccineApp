using CostelloMVCWebProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostelloClassLibrary;

namespace CostelloMVCWebProject.Models
{
    public class VaccineShipmentRepo : IVaccineShipmentRepo
    {
        private readonly ApplicationDbContext database;

        public VaccineShipmentRepo(ApplicationDbContext dbcontext)
            {
            this.database = dbcontext;

        }
        public List<VaccineShipment> ListAllVaccineShipments()
        {
            List<VaccineShipment> listofvaccineshipments = new List<VaccineShipment>();
            listofvaccineshipments = database.VaccineShipment.Include(vs=> vs.Vaccine).Include(vs=> vs.Facility).ToList();
            return listofvaccineshipments;
        }
        

        public int AddVaccineShipment(VaccineShipment vaccineshipment)
        {
            database.VaccineShipment.Add(vaccineshipment);
            database.SaveChanges();
            return vaccineshipment.VaccineShipmentID;

        }

        public VaccineShipment FindVaccineShipment(int? vaccineShipmentID)
        {
            VaccineShipment vaccineshipment = database.VaccineShipment.Find(vaccineShipmentID);

            return vaccineshipment;
        }

        public void EditVaccineShipment(VaccineShipment vaccineshipment)
        {
            database.VaccineShipment.Update(vaccineshipment);
            database.SaveChanges();
        }
    }
}
