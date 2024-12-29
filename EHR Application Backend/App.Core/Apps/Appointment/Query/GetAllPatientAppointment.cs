using App.Core.Interface;
using Domain.Entity.Appointments;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Domain.Entity.AuthProcess;
using Domain.Model;

namespace App.Core.Apps.Appointment.Query
{
    public class GetAllPatientAppointment:IRequest<object>
    {
        public int PatientId { get; set; }
    }
    public class GetAllPatientAppointmentHandler:IRequestHandler<GetAllPatientAppointment,object>
    {
        public readonly IAppDbContext _appDbContext;
        public GetAllPatientAppointmentHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetAllPatientAppointment request, CancellationToken cancellationToken)
        {
            var apts = new List<UserListDto>();
    
           
            var patientAppointment = await _appDbContext.Set<Domain.Entity.Appointments.Appointment>()
                .Where(a => a.PatientId == request.PatientId && a.Status == "scheduled")
                .OrderByDescending(a=>a.AppointmentDate)
                .ThenByDescending(a=>a.AppointmentTime)
                .ToListAsync();

        
            foreach(var apt in patientAppointment)
            {
                var userData = await _appDbContext.Set<Domain.Entity.AuthProcess.User>
                          ().FirstOrDefaultAsync(u => u.Id == apt.ProviderId);

                apts.Add(new UserListDto
                {
                    AppointmentDate = apt.AppointmentDate,
                    AppointmentTime = apt.AppointmentTime,
                    ProviderName = userData.FirstName + " " + userData.LastName,
                    PatientId=userData.Id,
                    ProviderId=userData.Id,
                    Fees = apt.Fees,
                    Status = apt.Status,
                    ChiefComplaint=apt.ChiefComplaint,
                    AppointmentId=apt.Id
                });
            }
            var response = new
            {
                staus = 200,
                message = "Patient Data by appointment",
                patientData = apts
            };
            return response;
           
        }
    }
}
