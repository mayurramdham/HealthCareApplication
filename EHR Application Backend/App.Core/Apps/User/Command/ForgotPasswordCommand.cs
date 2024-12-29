using App.Core.Apps.Helper;
using App.Core.Interface;
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
    public class ForgotPasswordCommand : IRequest<object>
    {
        public string Email { get; set; } 
    }
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public ForgotPasswordCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }

        public async Task<object> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var emailcheck=request.Email;
            var user = await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
               .FirstOrDefaultAsync(u => u.Email == emailcheck, cancellationToken);

            if (user == null)
            {
                return new
                {
                    status = 404,
                    message = "Invalid email. No user found with this email."
                };
              
            }
            string newPassword = ForgetPassword.GenerateRandomPassword(8);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            // Send new password via email

            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            max-width: 600px;
            margin: 20px auto;
            background: #ffffff;
            border: 1px solid #ddd;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }}
        .header {{
            background-color: #4CAF50;
            color: white;
            text-align: center;
            padding: 20px;
            font-size: 24px;
        }}
        .content {{
            padding: 20px;
            color: #333333;
        }}
        .content p {{
            font-size: 16px;
            line-height: 1.5;
            margin: 10px 0;
        }}
        .footer {{
            background-color: #f1f1f1;
            text-align: center;
            padding: 10px;
            font-size: 14px;
            color: #777777;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            margin: 20px 0;
            font-size: 16px;
            color: white;
            background-color: #4CAF50;
            text-decoration: none;
            border-radius: 4px;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            Password Reset
        </div>
        <div class='content'>
            <p>Dear {user.FirstName},</p>
            <p>Your password has been reset successfully. Please find your new password below:</p>
            <p><strong>{newPassword}</strong></p>
            <p>For security reasons, we recommend changing your password after logging in.</p>
            <a href='https://yourwebsite.com/login' class='button'>Login Now</a>
        </div>
        <div class='footer'>
            &copy; {DateTime.Now.Year} YourCompanyName. All rights reserved.
        </div>
    </div>
</body>
</html>
";

            var emailSent = await _emailService.SendEmailAsync(
                user.Email,
                "Password Reset",
                "Your new password",
                emailBody);

            if (!emailSent)
            {
                return new
                {
                    status = 500,
                    message = "Failed to send email. Please try again later."
                };
            }

            return new
            {
                status = 200,
                message = "New password sent successfully to your email."
            };
        }
       

    }
}
