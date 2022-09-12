using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostelloClassLibrary;

namespace CostelloMVCWebProject.Models
{
    public interface IVaccineShipmentRepo
    {
        //Method signatures (what needs to be done, not how it should be done or implemented)
        public List<VaccineShipment> ListAllVaccineShipments();
        int AddVaccineShipment(VaccineShipment vaccineshipment);
        VaccineShipment FindVaccineShipment(int? vaccineShipmentID);
        void EditVaccineShipment(VaccineShipment vaccineshipment);
    }
}
