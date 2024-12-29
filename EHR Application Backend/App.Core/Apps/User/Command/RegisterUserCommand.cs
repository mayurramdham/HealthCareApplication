using App.Core.Interface;
using Domain.Model;
using Domain.Model.AuthProcessDto;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace App.Core.Apps.User.Command
{
    public class RegisterUserCommand : IRequest<object>
    {
        public UserDto User { get; set; }
    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, object>
    {

        public readonly IAppDbContext _apppDbContext;
        public readonly IImageService _imageService;
        public readonly IEmailService _emailService;
        public RegisterUserCommandHandler(IAppDbContext appDbContext, IImageService imageService, IEmailService emailService)
        {
            _apppDbContext = appDbContext;
            _imageService = imageService;
            _emailService = emailService;
        }
        public async Task<object> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userRequest = request.User;
            var curentUser = await _apppDbContext.Set<Domain.Entity.AuthProcess.User>()
                            .FirstOrDefaultAsync(u => u.Email == userRequest.Email);
            if (curentUser != null)
            {
                return (new { status = 409, message = "User Already Exists." });
            }
            var imageUploadResult = await _imageService.Upload(userRequest.ProfileImageUrl);
            if (imageUploadResult is ResponseDto uploadResponse && uploadResponse.Status != 200)
            {
                return uploadResponse;
            }

            string uploadedImageUrl = (imageUploadResult as ResponseDto)?.Data?.ToString();

            var name = GenerateUsername(userRequest.UserTypeId, userRequest.FirstName, userRequest.LastName, userRequest.DateOfBirth);
            string plainPassword = GenerateRandomPassword();
            var newUser = userRequest.Adapt<Domain.Entity.AuthProcess.User>();

            var userExist = await _apppDbContext.Set<Domain.Entity.AuthProcess.User>().FirstOrDefaultAsync(user => user.UserName == name);

            if (userExist != null)
            {
                name = $"{userExist.UserName}1";
            }
            newUser.UserName = name;
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword, 13);
            newUser.Password = hashPassword;
            newUser.ProfileImageUrl = uploadedImageUrl;

            //var text = $"Hello,\n\nYour account has been successfully created. Below are your login details:\n\n" +
            //          $"Username: {name}\n" +
            //          $"Password: {plainPassword}\n\n" +
            //          "Please make sure to change your password after your first login for security reasons.\n\n" +
            //          "Best regards,\nThe Team";

            var text = $@"
<!DOCTYPE html>
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333; margin: 0; padding: 0;'>
    <div style='max-width: 600px; margin: 20px auto; background: #ffffff; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);'>
        <div style='background: #007BFF; color: #ffffff; text-align: center; padding: 10px 0; border-radius: 8px 8px 0 0;'>
            <h1 style='margin: 0; font-size: 24px;'>Welcome to HealthCare App</h1>
        </div>
        <div style='padding: 20px;'>
            <p>Hello {userRequest.FirstName},</p>
            <p>Your account has been successfully created. Below are your login details:</p>
            <div style='background: #f9f9f9; padding: 10px; border: 1px solid #e0e0e0; border-radius: 6px;'>
                <p><strong>Username:</strong> {name}</p>
                <p><strong>Password:</strong> {plainPassword}</p>
            </div>
            <p>Please make sure to change your password after your first login for security reasons.</p>
        </div>
        <div style='margin-top: 20px; text-align: center; font-size: 12px; color: #666;'>
            <p>Best regards,</p>
            <p>The HealthCare App Team</p>
        </div>
    </div>
</body>
</html>";



            var isSend = await _emailService.SendEmailAsync(newUser.Email, "User ", "Login Credentials", text);
            DateTime now = DateTime.Now;
            DateTime twoMinutesLater = now.AddMinutes(15);


            await _apppDbContext.Set<Domain.Entity.AuthProcess.User>().AddAsync(newUser);
            await _apppDbContext.SaveChangesAsync(cancellationToken);



            string GenerateRandomPassword()
            {
                return Guid.NewGuid().ToString("N").Substring(0, 8);
            }
            string GenerateUsername(int UserType, string firstName, string lastName, DateTime dob)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                if (UserType == 1)
                {
                    return $"PT_{textInfo.ToTitleCase(firstName)}{lastName.ToUpper()[0]}{dob:ddMMyy}";
                }
                else
                {
                    return $"PR_{textInfo.ToTitleCase(firstName)}{lastName.ToUpper()[0]}{dob:ddMMyy}";

                }
            }

            var response = new
            {
                status = 200,
                message = "User Regitstered Successfully",
                data = newUser,
                mail = text
            };
            return response;



        }
    }
}
