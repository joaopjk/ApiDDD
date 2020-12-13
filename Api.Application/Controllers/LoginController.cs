using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase 
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login ([FromBody] LoginDto userEntity,[FromServices] ILoginService service)
        {
            if(!ModelState.IsValid || userEntity == null) return BadRequest(ModelState);
            try
            {
                var result = await service.FindByLogin(userEntity);

                if (result != null) return Ok(result);
                else return NotFound();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}