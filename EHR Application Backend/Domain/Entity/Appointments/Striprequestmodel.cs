using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Appointments
{
    public class Striprequestmodel
    {
        public int Id { get; set; }
        public string SourceToken { get; set; }
        public float Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
