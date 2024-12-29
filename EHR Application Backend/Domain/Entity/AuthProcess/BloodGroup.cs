using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class BloodGroup
    {
        [Key]
        public int BloodgroupId { get; set; }
        public string BloodGroupType { get; set; }

    }
}
