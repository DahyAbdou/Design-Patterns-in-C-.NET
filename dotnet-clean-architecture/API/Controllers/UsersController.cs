using Application.Abstraction;
using Application.Implementation;
using Application.Request.User;
using Application.Response.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
    

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("user/by-id")]
        public async Task<ActionResult<UserDTO>> Get([FromQuery] GetUserByIdRequest request)
        {
            var user = await _userService.GetUserByIdAsync(request);

            return Ok(user);
        }
        [HttpGet("user/UserInfo")]
        public async Task<ActionResult<UserDTO>> GetUserById([FromQuery] GetUserByIdRequest request)
        {
            var user = await _userService.GetUserByIdUsingGenericRepositoryAndUnitOfWorkAsync(request);

            return Ok(user);
        }
    }
}
