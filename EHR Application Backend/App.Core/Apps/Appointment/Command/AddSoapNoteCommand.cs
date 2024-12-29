using App.Core.Interface;
using Domain.Model;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Appointment.Command
{
    public class AddSoapNoteCommand:IRequest<object>
    {
        public SoapNoteDto SoapNoteDto { get; set; }
    }
    public class AddSoapNoteCommandHandler : IRequestHandler<AddSoapNoteCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        public AddSoapNoteCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(AddSoapNoteCommand request, CancellationToken cancellationToken)
        {
            var soapNote=request.SoapNoteDto;
            var appointment=await _appDbContext.Set<Domain.Entity.Appointments.Appointment>()
                                             .FirstOrDefaultAsync(s=>s.Id == soapNote.SoapNoteId);

            if (appointment == null)
            {
                return new { status = 404, message = "appointment not found with this id" };

            }
            var soapnotsss = new Domain.Entity.Appointments.SoapNoteTable
            {
                subjective = soapNote.subjective,
                assessment = soapNote.assessment,
                objective = soapNote.objective,
                plan = soapNote.plan,
                AppointmentId = appointment.Id,
                Status = "Completed"

            };
            //var apWithSoap= soapNote.Adapt(appointment);

            await _appDbContext.Set<Domain.Entity.Appointments.SoapNoteTable>()
               .AddAsync(soapnotsss);


            appointment.Status = "Completed";
           // await _appDbContext.Set<Domain.Entity.Appointments.SoapNoteTable>().AddAsync(apWithSoap, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            var response = new
            {
                status = 200,
                message = "Soap note added successfully",
                soapData = appointment
            };
            return response;
            
        }
    }
}
