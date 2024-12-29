using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ResponseDto
    {
        public int Status { get; set; } // Represents HTTP status code
        public string Message { get; set; } // Contains the message to describe the response
        public string Data { get; set; } // Contains additional data (in your case, it's an empty string)
    }
}
