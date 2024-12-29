using App.Core.Interface;
using Domain.Entity.AuthProcess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.DropDown
{
    public class GetProviderBySpecialityQuery : IRequest<object>
    {
        public int Id { get; set; }
    }
       

        public class GetProviderBySpecialityQueryHandler : IRequestHandler<GetProviderBySpecialityQuery, object>
        {
            private readonly IAppDbContext _appDbContext;

            public GetProviderBySpecialityQueryHandler(IAppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }

            public async Task<object> Handle(GetProviderBySpecialityQuery request, CancellationToken cancellationToken)
            {
                var providersdata = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                    .Where(u => u.SpecialisationId == request.Id)
                    .Select(u => new
                    {
                        id = u.Id, // Assuming `UserId` is the unique identifier for the provider
                        FirstName = u.FirstName, // Replace `Name` with the actual property for the provider's name
                        VisitingCharge=u.VisitingCharge
                    })
                    .ToListAsync(cancellationToken);

                var response = new
                {
                    status = 200,
                    message = "All providers filtered by SpecialisationId",
                    providers = providersdata
                };

                return response;
            }
        }
    }
