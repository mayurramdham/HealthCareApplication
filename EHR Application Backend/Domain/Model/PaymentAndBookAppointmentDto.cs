using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class PaymentAndBookAppointmentDto
    {
        public DateTime AppointmentDate { get; set; } 
        public int ProviderId { get; set; } 
        public int PatientId { get; set; } 
        public string ChiefComplaint { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public float Amount { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string SourceToken { get; set; }

      
    }

}
