using Domain.Entity.Appointments;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interface
{
    public interface IStripePaymentService
    {
        Task<StripePaymentResponse> CreateStripePayment(Striprequestmodel striprequestmodel);
    }

}
