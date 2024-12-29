using App.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Appointment.Query
{
    public class GetPatientAppointmentByAppointmentId:IRequest<object>
    {
        public int AppointmentId { get; set; }
    }
    public class GetPatientAppointmentByAppointmentIdHandler:IRequestHandler<GetPatientAppointmentByAppointmentId,object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetPatientAppointmentByAppointmentIdHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetPatientAppointmentByAppointmentId request, CancellationToken cancellationToken)
        {
            var appId = request.AppointmentId;
            if(appId ==0)
            {
                return "appointment not found with the id ";
            }
            var appInfo = await _appDbContext.Set<Domain.Entity.Appointments.Appointment>()
                                        .FirstOrDefaultAsync(a => a.Id == appId);
            var userInfo= await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                                        .FirstOrDefaultAsync(u => u.Id == appInfo.PatientId);

            var response = new
            {
                status = 200,
                message = "Appointment data for profile",
                appData = appInfo,
                userData = userInfo
            };
            return response;





        }
    }
}
