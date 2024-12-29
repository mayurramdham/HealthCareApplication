using Domain.Entity.Register;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Register
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; } 
        public string Shortname { get; set; }
        public string CountryName { get; set; }
        public string PhoneCode { get; set; }
        public List<State> State { get; set; }
    }
}
