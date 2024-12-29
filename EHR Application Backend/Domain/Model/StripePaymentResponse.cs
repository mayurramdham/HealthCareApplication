using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class StripePaymentResponse
    {
        public string Id { get; set; } // PaymentIntent ID
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
