using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Register
{
    public class Otp
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public int OtpCode { get; set; }
        public string email { get; set; }
        public DateTime Expiration {  get; set; }

    }
}
