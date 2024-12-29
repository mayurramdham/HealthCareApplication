using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class SoapNoteDto
    {
        public int SoapNoteId { get; set; }
        public string subjective { get; set; }
        public string objective { get; set; }
        public string assessment { get; set; }
        public string plan { get; set; }
    }
}
