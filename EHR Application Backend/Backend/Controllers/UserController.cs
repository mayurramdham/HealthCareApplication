using App.Core.Apps.User.Command;
using App.Core.Apps.User.Query;
using Domain.Model.AuthProcessDto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("RegisterProvider")]
        public async Task<IActionResult> registerProvider(UserDto user)
        {
            var userResponse = await _mediator.Send(new RegisterUserCommand { User = user });
            return Ok(userResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Otp([FromBody] LoginDto logins)
        {
            var otp = await _mediator.Send(new LoginCommand  { LoginDto = logins });
            return Ok(otp);
        }
        [HttpPost("verifyotp")]
        public async Task<IActionResult> login([FromBody] OtpDto otp)
        {
            var login = await _mediator.Send(new VeryOtpCommand { otpDto = otp });
            return Ok(login);
        }
        [HttpPost("forgetPassword")]
        public async Task<IActionResult> forgetPassword([FromBody] EmailDto email)
        {
            var forgerPasswordResponse=await _mediator.Send(new ForgotPasswordCommand { Email = email.Email });


            return Ok(forgerPasswordResponse);
        }

        [HttpGet("getUserById/{Id}")]
        public async Task<IActionResult> getUser(int Id)
        {
            var getUser=await _mediator.Send(new getUserById { Id = Id });
            return Ok(getUser);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> changePassword(ChangePasswordDto changePassword)
        {
            var changePass=await _mediator.Send(new  ChangePasswordCommand { ChangePassword = changePassword });
            return Ok(changePass);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> updateProfile(UpdateUserDto updateUserDto)
        {
            var updateUser = await _mediator.Send(new UpdateUserProfileCommand { userDto = updateUserDto });
            return Ok(updateUser);
        }


    }
}
