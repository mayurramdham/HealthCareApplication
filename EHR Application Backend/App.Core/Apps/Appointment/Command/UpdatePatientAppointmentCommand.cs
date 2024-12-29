using App.Core.Interface;
using Domain.Model.AuthProcessDto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Appointment.Command
{
    public class UpdatePatientAppointmentCommand:IRequest<object>
    {
        public UpdateAppointmentDto updateAppointmentDto {  get; set; }
    }
    public class UpdatePatientAppointmentCommandHandler : IRequestHandler<UpdatePatientAppointmentCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        public UpdatePatientAppointmentCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(UpdatePatientAppointmentCommand request, CancellationToken cancellationToken)
        {
            var updateAppointmentData=request.updateAppointmentDto;
            var updateAppintment=await _appDbContext.Set<Domain.Entity.Appointments.Appointment>()
                                                    .FirstOrDefaultAsync(a=>a.Id==updateAppointmentData.appointmentId);
            if (updateAppintment is null)
            {
                return new { status = 404, message = "Patient with the appointment id not found" };
            }
            updateAppintment.AppointmentTime=updateAppointmentData.AppointmentTime;
            updateAppintment.AppointmentDate=updateAppointmentData.AppointmentDate;
            updateAppintment.ChiefComplaint= updateAppointmentData.ChiefComplaint;
            await _appDbContext.SaveChangesAsync(cancellationToken);
            var response = new
            {
                status = 200,
                message = "appointment updated successfully",
                updated = updateAppintment
            };
            return response;
        
        }
    }

}
