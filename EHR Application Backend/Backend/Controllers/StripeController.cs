using App.Core.Apps.Appointment.Command;
using App.Core.Interface;
using Domain.Entity.Appointments;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IStripePaymentService _stripePaymentService;
        private readonly IMediator _mediator;
        public StripeController(IStripePaymentService stripePaymentService,IMediator mediator)
        {
            _stripePaymentService = stripePaymentService;
            _mediator = mediator;   
        }

        [HttpPost("[action]")]
        //[Authorize(Roles = "Patient, Provider")]
        public async Task<IActionResult> PaymentBookAppointment(PaymentAndBookAppointmentDto bookAppointmentDto)
        {
            var result = await _mediator.Send(new PaymentAndBookAppointmentCommand { paymentAndBookAppointmentDto = bookAppointmentDto });
            return Ok(result);
        }

    }
}
