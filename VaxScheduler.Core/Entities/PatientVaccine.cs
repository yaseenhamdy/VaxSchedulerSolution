using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxScheduler.Core.Entities
{
    public class PatientVaccine
    {
        public Patient Patient { get; set; }
        public int PatientId { get; set; }


        public Vaccine Vaccine { get; set; }
        public int VaccineId { get;set; }
    }
}
