using App.Core.Interface;
using Domain.Model.AuthProcessDto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Appointment.Command
{
    public class CancelAppointmentCommand:IRequest<object>
    {
        public int AppointmentId { get; set; }
    }
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public CancelAppointmentCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }

        public async Task<object> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var patientAppointment = await _appDbContext.Set<Domain.Entity.Appointments.Appointment>()
                                                       .FirstOrDefaultAsync(a => a.Id == request.AppointmentId);

            var ptEmail = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                              .FirstOrDefaultAsync(u => u.Id == patientAppointment.PatientId);

            var prEmail = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                .FirstOrDefaultAsync(u => u.Id == patientAppointment.ProviderId);
            var patientMessage = "Your appointment with provider has been cancled by the provider due to some reason";
            var providerMessage = "Your appointment with patient has been cancled by the patient due to some reason";

            patientAppointment.Status = "canceled";
            var patientEmail = await _emailService.SendEmailAsync(ptEmail.Email, "Patient", "Mail About AppointmentStatus", patientMessage);
            var providerEmail = await _emailService.SendEmailAsync(prEmail.Email, "Provider", "Your Appointment is Scheduled with the patient", providerMessage);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            var response = new
            {
                status = 200,
                message = "appointment canceled successfully",
                appointment=patientAppointment

            };

            return response;
        }
    }
}
