using CostelloClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Models
{
    public interface IVaccineExchangeRepo
    {
        public List<VaccineExchange> ListAllVaccineExchanges();
        public int MakeVaccineExhange(VaccineExchange vaccineexchange);
    }
}
