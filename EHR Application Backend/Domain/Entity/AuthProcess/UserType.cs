using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Register
{
    public class UserType
    {
        [Key]
       public int UserTypeId { get; set; }
       public string UserTypeName { get; set; }
       
    }
}
 