using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _service.GetAll());
            }
            catch (ArgumentException ex)
            {
                return StatusCode ((int) HttpStatusCode.InternalServerError,ex.Message);
            }
        }
        [Authorize("Bearer")]
        [HttpGet]
        [Route("{id}",Name ="GetById" )]
        public async Task<ActionResult> Get (Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return Ok(await _service.Get(id));
            }
            catch (ArgumentException ex)
            {
                return StatusCode ((int) HttpStatusCode.InternalServerError,ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserDtoCreate user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

             try
            {
               var result = await _service.Post(user);
               if(result != null) return Created(new Uri(Url.Link("GetById", new {id = result.Id})),result);
               else return BadRequest();
            }
            catch (ArgumentException ex)
            {
                return StatusCode ((int) HttpStatusCode.InternalServerError,ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserDtoUpdate user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

             try
            {
               var result = await _service.Put(user);
               if(result != null) return Ok(result);
               else return BadRequest();
            }
            catch (ArgumentException ex)
            {
                return StatusCode ((int) HttpStatusCode.InternalServerError,ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return Ok(await _service.Delete(id));
            }
            catch (ArgumentException ex)
            {
                return StatusCode ((int) HttpStatusCode.InternalServerError,ex.Message);
            }
        }
    
    }
}