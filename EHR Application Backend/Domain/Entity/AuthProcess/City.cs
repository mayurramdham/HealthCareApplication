using Domain.Entity.Register;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.AuthProcess
{
    public class City
    {
        [Key]
        public int CityId { get; set; } 
        public string CityName { get; set; }
        [ForeignKey("State")]
        public int StateId { get; set; }
        public State State { get; set; }    
    }
}
