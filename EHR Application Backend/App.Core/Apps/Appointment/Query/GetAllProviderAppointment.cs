using App.Core.Interface;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Appointment.Query
{
    public class GetAllProviderAppointment : IRequest<object>
    {
        public int PatientId { get; set; }
    }
    public class GetAllProviderAppointmentHandler : IRequestHandler<GetAllProviderAppointment, object>
    {
        public readonly IAppDbContext _appDbContext;
        public GetAllProviderAppointmentHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetAllProviderAppointment request, CancellationToken cancellationToken)
        {
            var apts = new List<UserListDto>();


            var patientAppointment = await _appDbContext.Set<Domain.Entity.Appointments.Appointment >()
                .Where(a => a.ProviderId == request.PatientId && a.Status == "scheduled")
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();

            //&& a.Status == "scheduled"
            foreach (var apt in patientAppointment)
            {
                var userData = await _appDbContext.Set<Domain.Entity.AuthProcess.User>
                          ().FirstOrDefaultAsync(u => u.Id == apt.ProviderId);
                var patientData = await _appDbContext.Set<Domain.Entity.AuthProcess.User>
                          ().FirstOrDefaultAsync(u => u.Id == apt.PatientId);

                apts.Add(new UserListDto
                {
                    AppointmentDate = apt.AppointmentDate,
                    AppointmentTime = apt.AppointmentTime,
                    PatientName = patientData.FirstName + " " + patientData.LastName,
                    ProviderId=patientData.Id,
                    Fees = apt.Fees,
                    Status = apt.Status,
                    ChiefComplaint = apt.ChiefComplaint,
                    AppointmentId = apt.Id
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
