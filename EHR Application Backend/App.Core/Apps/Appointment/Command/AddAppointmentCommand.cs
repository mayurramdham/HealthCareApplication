using App.Core.Interface;
using Domain.Model.AuthProcessDto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Apps.Appointment.Command
{
    public class AddAppointmentCommand : IRequest<object>
    {
        public AppointmentDto AppointmentDto { get; set; }
    }
    public class AddAppointmentCommandHandler : IRequestHandler<AddAppointmentCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public AddAppointmentCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }

        public async Task<object> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointmentData = request.AppointmentDto;
            

            var prEmail = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                .FirstOrDefaultAsync(u => u.Id == appointmentData.ProviderId);

            var ptEmail = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                                .FirstOrDefaultAsync(u => u.Id == appointmentData.PatientId);

            var appointment = new Domain.Entity.Appointments.Appointment
            {
                PatientId = request.AppointmentDto.PatientId,
                ProviderId = request.AppointmentDto.ProviderId,
             //   SpecialisationId = request.AppointmentDto.SpecialisationId,
                SpecialisationId = prEmail.SpecialisationId,
                AppointmentDate = request.AppointmentDto.AppointmentDate,
                AppointmentTime = request.AppointmentDto.AppointmentTime,
                ChiefComplaint = request.AppointmentDto.ChiefComplaint,
                //Fees = request.AppointmentDto.Fees,
                Fees = prEmail.VisitingCharge,
                Status = "scheduled"
            };
            var patientMessage = $"Dear {ptEmail.FirstName},\n\n" +
                                "Welcome to our healthcare platform! We are pleased to inform you that your account has been successfully created.\n\n" +
                                "To ensure your account remains secure, we strongly recommend that you change your password after your first login.\n\n" +
                                "Additionally, we have scheduled an appointment for you with the following details:\n\n" +
                                $"Appointment Date: {request.AppointmentDto.AppointmentDate}\n" +
                                $"Appointment Time: {request.AppointmentDto.AppointmentTime}\n" +
                                $"Chief Complaint: {request.AppointmentDto.ChiefComplaint}\n" +
                                $"Appointment Fees: {request.AppointmentDto.Fees}\n\n" +
                                "If you need to reschedule or have any questions, please do not hesitate to contact our support team.\n\n" +
                               "Thank you for choosing our services. We look forward to providing you with the best care possible.\n\n" +
                               "Best regards,\nThe [Your Healthcare Platform] Team";

            var providerMessage = $"Dear Dr. {prEmail.FirstName},\n\n" +
"We are pleased to inform you that a new appointment has been scheduled for one of your patients. Below are the details for the upcoming appointment:\n\n" +
$"Patient Name: {ptEmail.FirstName}\n" +
$"Appointment Date: {request.AppointmentDto.AppointmentDate}\n" +
$"Appointment Time: {request.AppointmentDto.AppointmentTime}\n" +
$"Chief Complaint: {request.AppointmentDto.ChiefComplaint}\n" +
$"Appointment Fees: {request.AppointmentDto.Fees}\n\n" +
"Please ensure that you are available at the scheduled time. If you need to reschedule or have any questions regarding this appointment, feel free to contact our support team.\n\n" +
"Thank you for your continued partnership in providing the best care to our patients.\n\n" +
"Best regards,\nThe [Your Healthcare Platform] Team";
        //    var patientEmail = await _emailService.SendEmailAsync(ptEmail.Email, "Patient", "Your Appointment is Scheduled with the provider", patientMessage);
         //   var providerEmail = await _emailService.SendEmailAsync(prEmail.Email, "Provider", "Your Appointment is Scheduled with the patient", providerMessage);
            await _appDbContext.Set<Domain.Entity.Appointments.Appointment>().AddAsync(appointment, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            var response = new
            {
                status = 200,
                message = "Appointment Booked successfully",
                data = appointment
            };
            return response;


        }
    }
}
