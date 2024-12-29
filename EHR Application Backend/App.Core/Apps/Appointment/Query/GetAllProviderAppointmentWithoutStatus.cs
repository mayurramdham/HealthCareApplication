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
    public class GetAllProviderAppointmentWithoutStatus : IRequest<object>
    {
        public int ProviderId { get; set; }
    }
    public class GetAllProviderAppointmentWithoutStatusHanlder : IRequestHandler<GetAllProviderAppointmentWithoutStatus, object>
    {
        public readonly IAppDbContext _appDbContext;
        public GetAllProviderAppointmentWithoutStatusHanlder(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetAllProviderAppointmentWithoutStatus request, CancellationToken cancellationToken)
        {
            var apts = new List<UserListDto>();


            var patientAppointment = await _appDbContext.Set<Domain.Entity.Appointments.Appointment>()
                .Where(a => a.ProviderId == request.ProviderId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();


            foreach (var apt in patientAppointment)
            {
                var userData = await _appDbContext.Set<Domain.Entity.AuthProcess.User>
                          ().FirstOrDefaultAsync(u => u.Id == apt.PatientId);

                apts.Add(new UserListDto
                {
                    AppointmentDate = apt.AppointmentDate,
                    AppointmentTime = apt.AppointmentTime,
                    ProviderName = userData.FirstName + " " + userData.LastName,
                    Fees = apt.Fees,
                    Status = apt.Status,
                    ChiefComplaint = apt.ChiefComplaint,
                    AppointmentId = apt.Id
                });
            }
            var response = new
            {
                staus = 200,
                message = "Provider Data by appointment without status",
                patientData = apts
            };
            return response;

        }
    }
}
