using CostelloClassLibrary;
using CostelloMVCWebProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CostelloMVCWebProject.Models
{
    public class VaccineExchangeRepo : IVaccineExchangeRepo
    {
        private readonly ApplicationDbContext database;

        public VaccineExchangeRepo(ApplicationDbContext dbcontext)
        {
            this.database = dbcontext;

        }

        public List<VaccineExchange> ListAllVaccineExchanges()
        {
            List<VaccineExchange> listofexchanges = new List<VaccineExchange>();
            listofexchanges = database.VaccineExchange.Include(vs => vs.Vaccine).Include(vs => vs.SendingFacility).Include(vs=>vs.ReceivingFacility).ToList();
            return listofexchanges;
        }
        public int MakeVaccineExhange(VaccineExchange vaccineexchange)
        {
            database.VaccineExchange.Add(vaccineexchange);
            database.SaveChanges();
            return vaccineexchange.VaccineExchangeID;
        }
    }
}
