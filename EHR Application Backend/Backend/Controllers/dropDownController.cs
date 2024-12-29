using App.Core.Apps.DropDown;
using EllipticCurve;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dropDownController : ControllerBase
    {
        private readonly IMediator _mediator;
        public dropDownController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetCountries")]
        public async Task<ActionResult> GetCountries()
        {
            var result = await _mediator.Send(new GetCountriesQuery());
            return Ok(result);
        }
        [HttpGet("GetStatesByCountry/{countryId}")]
        public async Task<ActionResult> GetStatesByCountry(int countryId)
        {
            var result = await _mediator.Send(new GetStatesByCountryQuery(countryId));
            return Ok(result);
        }
        [HttpGet("GetCityByState/{stateId}")]
        public async Task<IActionResult> GetCityiesfromCountry(int stateId)
        {
            var result = await _mediator.Send(new GetCityByStateQuery { CityId = stateId });
            return Ok(result);
        }
    }
}
