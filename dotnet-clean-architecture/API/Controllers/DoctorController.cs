using Application.Abstraction.Services;
using Application.Request.DoctorJobTitle;
using Application.Request.User;
using Application.Response.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorJobTitleService _doctorJobTitleService;

        public DoctorController(IDoctorJobTitleService doctorJobTitleService)
        {
            _doctorJobTitleService = doctorJobTitleService;
        }
        [HttpGet("DoctorJobTitle/by-JobTitle")]
        public async Task<ActionResult<UserDTO>> Get([FromQuery] DoctorJobTitleRequest request)
        {
            var DoctorJobTitle = await _doctorJobTitleService.GetDoctorJobTitleAsync(request);

            return Ok(DoctorJobTitle);
        }
    }
}
