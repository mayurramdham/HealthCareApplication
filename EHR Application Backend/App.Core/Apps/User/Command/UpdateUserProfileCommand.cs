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
    public class UpdateUserProfileCommand : IRequest<object>
    {
        public UpdateUserDto userDto { get; set; }
    }
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IImageService _imageService;
        public UpdateUserProfileCommandHandler(IAppDbContext appDbContext, IImageService imageService)
        {
            _appDbContext = appDbContext;
            _imageService = imageService;
        }

        public async Task<object> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userToUpdate = request.userDto;
            var updateUserId = await _appDbContext.Set<Domain.Entity.AuthProcess.User>().
                                     FirstOrDefaultAsync(u => u.Id == request.userDto.UserId);

            if (updateUserId is null) return new
            {
                status = 404,
                message = "User not found in the database",
                data = updateUserId
            };

            // var imageUploadResult = await _imageService.Upload(userToUpdate.ProfileImage);
            // if (imageUploadResult is ResponseDto uploadResponse && uploadResponse.Status != 200)
            // {
            //     return uploadResponse;
            //}

            //  string uploadedImageUrl = (imageUploadResult as ResponseDto)?.Data?.ToString();


            //  updateUserId.ProfileImage = uploadedImageUrl;
            updateUserId.DateOfBirth = userToUpdate.DateOfBirth;
            updateUserId.Mobile = userToUpdate.Mobile;
            updateUserId.Address = userToUpdate.Address;
            updateUserId.FirstName = userToUpdate.FirstName;
            updateUserId.LastName = userToUpdate.LastName;
            updateUserId.PinCode = userToUpdate.PinCode;
            updateUserId.Email = userToUpdate.Email;

            await _appDbContext.SaveChangesAsync(cancellationToken);
            var response = new
            {
                status = 200,
                message = "User updated successfully",
                updateUser = updateUserId
            };
            return response;
        }

    }
}
