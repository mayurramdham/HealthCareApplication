using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.AuthProcessDto
{
    public class OtpDto
    {
        public string UserName { get; set; }
        public int OtpCode { get; set; }
        public int UserId { get; set; }
    }
}
