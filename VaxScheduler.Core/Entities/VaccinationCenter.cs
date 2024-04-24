using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;

namespace VaxScheduler.Core.Entities
{
    public class VaccinationCenter : BaseEntity
    {
      
        public string Name { get; set; }

        [EmailAddress]
		public string Email { get; set; }


		public string Location { get; set; }

        public  Admin Admin { get; set; }
        public int AdminId { get; set; }


        public ICollection<VaccineVaccinationCenter> VaccineVaccinationCenter { get; set; }

    }
}
