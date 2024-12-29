using App.Core.Interface;
using BCrypt.Net;
using Domain.Model.AuthProcessDto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.User.Command
{
    public class ChangePasswordCommand : IRequest<object>
    {
        public ChangePasswordDto ChangePassword { get; set; }
    }
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        public ChangePasswordCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
           var userRequest=request.ChangePassword;
            var checkUser = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                            .FirstOrDefaultAsync(u => u.Email == userRequest.Email);
            if (checkUser is null)
            {
                return "user not exits";
            }

            checkUser.Password =BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            var response = new
            {
                status = 200,
                messgae = "Your Password change Successfully",
                data = checkUser
            };
            return response;

        }
    }
}
