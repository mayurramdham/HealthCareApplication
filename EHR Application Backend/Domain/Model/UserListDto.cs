using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserListDto
    {
        public int AppointmentId { get; set; }
        public string Status { get; set; }
        public string ProviderName { get; set; }
        public string PatientName { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string ChiefComplaint { get; set; }
        public float Fees { get; set; }
    }
}
