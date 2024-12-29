using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.AuthProcessDto
{
    public class AppointmentDto
    {
   
   
        public int? PatientId { get; set; }
        public int? ProviderId { get; set; }
   
        public int? SpecialisationId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string ChiefComplaint { get; set; }
        public float Fees { get; set; }     
    }
}
