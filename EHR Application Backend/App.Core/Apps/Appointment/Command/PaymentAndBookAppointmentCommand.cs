using App.Core.Interface;
using Domain.Entity.Appointments;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace App.Core.Apps.Appointment.Command
{
    public class PaymentAndBookAppointmentCommand : IRequest<object>
    {
        public PaymentAndBookAppointmentDto paymentAndBookAppointmentDto { get; set; }
    }
    internal class PaymentAndBookAppointmentCommandHandler : IRequestHandler<PaymentAndBookAppointmentCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IStripePaymentService _stripePaymentService;
        private readonly IEmailService _emailService;

        public PaymentAndBookAppointmentCommandHandler(IAppDbContext appDbContext, IStripePaymentService stripePaymentService,
            IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _stripePaymentService = stripePaymentService;
            _emailService = emailService;
        }

        public async Task<object> Handle(PaymentAndBookAppointmentCommand request, CancellationToken cancellationToken)
        {
            var bookAppointmentDto = request.paymentAndBookAppointmentDto;

            var appointmentDate = bookAppointmentDto.AppointmentDate;
            var providerId = bookAppointmentDto.ProviderId;
            var patientId = bookAppointmentDto.PatientId;
            var patient = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                      .FirstOrDefaultAsync(u => u.Id == patientId, cancellationToken);

            if (patient is null)
                return   new { staus = 404, msg = "User not found " };

            var provider = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                          .FirstOrDefaultAsync(u => u.Id == providerId, cancellationToken);

            if (provider is null)
                return new { staus = 404, msg = "User not found " };

            var newAppointment = new Domain.Entity.Appointments.Appointment()
            {
                AppointmentDate = appointmentDate,
                Status = "Scheduled",
                User = patient,
                PatientId = patient.Id,
                ChiefComplaint = bookAppointmentDto.ChiefComplaint,
                Fees = provider.VisitingCharge,
                AppointmentTime = bookAppointmentDto.AppointmentTime,
                ProviderId = providerId,
            };

            await _appDbContext.Set<Domain.Entity.Appointments.Appointment>()
                  .AddAsync(newAppointment);

            var paymentAndOrderDto = new Striprequestmodel()
            {
                Amount = bookAppointmentDto.Amount,
                CustomerEmail = bookAppointmentDto.CustomerEmail,
                CustomerName = bookAppointmentDto.CustomerName,
                SourceToken = bookAppointmentDto.SourceToken,
                Id = bookAppointmentDto.PatientId
            };

            
            var paymentInfo = await _stripePaymentService.CreateStripePayment(paymentAndOrderDto);
            if (paymentInfo is null)
            {
                return new { staus = 404, msg = "Your Payment Fail" };
            }

            await _appDbContext.SaveChangesAsync();

            var body = $@"
<!DOCTYPE html>
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333; margin: 0; padding: 0;'>
    <div style='max-width: 600px; margin: 20px auto; background: #ffffff; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);'>
        <div style='background: #007BFF; color: #ffffff; text-align: center; padding: 10px 0; border-radius: 8px 8px 0 0;'>
            <h1 style='margin: 0; font-size: 24px;'>Appointment Scheduled</h1>
        </div>
        <div style='padding: 20px;'>
            <p>Dear {patient.FirstName} {patient.LastName},</p>
            <p>Your appointment has been scheduled with Dr. {provider.FirstName} {provider.LastName}.</p>
            <p><strong>Appointment Details:</strong></p>
            <p><strong>Date:</strong> {appointmentDate}</p>
            <p><strong>Time:</strong> {newAppointment.AppointmentTime}</p>
            <p>We look forward to seeing you soon.</p>
        </div>
        <div style='margin-top: 20px; text-align: center; font-size: 12px; color: #666;'>
            <p>Best regards,</p>
            <p>Your Healthcare Provider</p>
        </div>
    </div>
</body>
</html>";

            var body2 = $@"
<!DOCTYPE html>
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333; margin: 0; padding: 0;'>
    <div style='max-width: 600px; margin: 20px auto; background: #ffffff; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);'>
        <div style='background: #007BFF; color: #ffffff; text-align: center; padding: 10px 0; border-radius: 8px 8px 0 0;'>
            <h1 style='margin: 0; font-size: 24px;'>Appointment Scheduled</h1>
        </div>
        <div style='padding: 20px;'>
            <p>Dear Dr. {provider.FirstName} {provider.LastName},</p>
            <p>Your appointment has been scheduled with {patient.FirstName} {patient.LastName}.</p>
            <p><strong>Appointment Details:</strong></p>
            <p><strong>Date:</strong> {appointmentDate}</p>
            <p><strong>Time:</strong> {newAppointment.AppointmentTime}</p>
            <p>We look forward to seeing you soon.</p>
        </div>
        <div style='margin-top: 20px; text-align: center; font-size: 12px; color: #666;'>
            <p>Best regards,</p>
            <p>Your Healthcare Provider</p>
        </div>
    </div>
</body>
</html>";





            await _emailService.SendEmailAsync(patient.Email, patient.FirstName,
               "Appointment Booking Confirmation", body);


            await _emailService.SendEmailAsync(provider.Email, provider.FirstName,
             "Appointment Booking Confirmation", body2);

            return new { status = 200, message = "Your Appointment Booked Successfully" };
        }
    }
}
