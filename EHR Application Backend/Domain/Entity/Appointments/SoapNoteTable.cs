using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Appointments
{
    public class SoapNoteTable
    {
        public int Id { get; set; }
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public string subjective { get; set; }
        public string objective { get; set; }   
        public string assessment { get; set; }
        public string plan { get; set; }
        public string Status { get; set; }
        public Appointment Appointment { get; set; }
    }
}
