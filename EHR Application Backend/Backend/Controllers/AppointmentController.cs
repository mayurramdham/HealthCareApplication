using App.Core.Apps.Appointment.Command;
using App.Core.Apps.Appointment.Query;
using App.Core.Apps.DropDown;
using Domain.Model;
using Domain.Model.AuthProcessDto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentController(IMediator mediator)
        {
            _mediator=mediator;
        }
        [HttpGet]
        public async Task<IActionResult> getspecialisation()
        {
            var speality = await _mediator.Send(new GetSpecilatyQuery());
            return Ok(speality);
        }

        [HttpGet("getSpecilisationById/{Id}")]
        public async Task<IActionResult> getProviderSpeciality(int Id)
        {
            var getUser = await _mediator.Send(new GetProviderBySpecialityQuery { Id = Id });
            return Ok(getUser);
        }

        [HttpPost("bookAppointment")]
        public async Task<IActionResult> addAppointment([FromBody] AppointmentDto appointments)
        {
            var appointment = await _mediator.Send(new AddAppointmentCommand { AppointmentDto = appointments });
            return Ok(appointment);
        }

        [HttpGet("getProvider/{UserTypeId}")]
        public async Task<IActionResult> getProviderById(int UserTypeId)
        {
            var provider=await _mediator.Send(new GetAllProvider { UserTypeId = UserTypeId });
            return Ok(provider);
        }

        [HttpGet("getAllPatient/{PatientId}")]
        public async Task<IActionResult> getPatientByPatientId(int PatientId)
        {
            var patientInfo = await _mediator.Send(new GetAllPatientAppointment { PatientId = PatientId });
            return Ok(patientInfo);
        }

        [HttpGet("getAllPatientWihoutStatus/{PatientId}")]
        public async Task<IActionResult> getPatientByPatientIdWihoutStatus(int PatientId)
        {
            var patientInfo = await _mediator.Send(new GetAllPatientAppointmentWithoutStatus { PatientId = PatientId });
            return Ok(patientInfo);
        }

        [HttpGet("getAllProviderWihoutStatus/{ProviderId}")]
        public async Task<IActionResult> getProviderByProviderIdWihoutStatus(int ProviderId)
        {
            var patientInfo = await _mediator.Send(new GetAllProviderAppointmentWithoutStatus { ProviderId = ProviderId });
            return Ok(patientInfo);
        }

        [HttpGet("getAllPatientByProvider/{PatientId}")]
        public async Task<IActionResult> getPatientByProviderId(int PatientId)
        {
            var providerInfo = await _mediator.Send(new GetAllProviderAppointment { PatientId = PatientId });
            return Ok(providerInfo);
        }

        [HttpDelete("DeleteAppointment/{AppointmentId}")]
        public async Task<IActionResult> ChangeAppointmentStatus(int AppointmentId)
        {
            var apStatus = await _mediator.Send(new CancelAppointmentCommand { AppointmentId = AppointmentId });
            return Ok(apStatus);
        }

        [HttpPut("UpdatePatientAppointment")]
        public async Task<IActionResult> updatePatientAppointment(UpdateAppointmentDto updateAppointmentDto)
        {
            var updateAp = await _mediator.Send(new UpdatePatientAppointmentCommand { updateAppointmentDto = updateAppointmentDto });
            return Ok(updateAp);
        }

        [HttpGet("getAppointmentById/{AppointmentId}")]
        public async Task<IActionResult> getProfileInfo(int AppointmentId)
        {
            var propInfo = await _mediator.Send(new GetPatientAppointmentByAppointmentId { AppointmentId = AppointmentId });
            return Ok(propInfo);
        }

        [HttpPost("AddSoapNote")]
        //[Authorize(Roles ="Provider")]
        public async Task<IActionResult> addSoapNotes(SoapNoteDto soapNoteDto)
        {
            var soapNote = await _mediator.Send(new AddSoapNoteCommand { SoapNoteDto = soapNoteDto });
            return Ok(soapNote);
        }
        


    }
}
