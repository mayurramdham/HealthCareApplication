using App.Core.Interface;
using Domain.Entity.AuthProcess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Appointment.Query
{
    public class GetSpecilatyQuery:IRequest<object>
    {
    }
    public class GetSpecilatyQueryHandler : IRequestHandler<GetSpecilatyQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetSpecilatyQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }

        public async Task<object> Handle(GetSpecilatyQuery request, CancellationToken cancellationToken)
        {
            var speciality = await _appDbContext.Set<Specialisation>().ToListAsync(cancellationToken);
            var response = new
            {
                status = 200,
                message = "get all doctor specality",
                data = speciality,
            };
            return response;
        }
    }
}
