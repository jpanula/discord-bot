using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Magic8BallController : ControllerBase
    {
        private IMagic8BallService _service;

        public Magic8BallController(IMagic8BallService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("random/weighted")]
        public IActionResult GetRandomWeighted()
        {
            return Ok(_service.GetRandomWeighted());
        }

        [HttpPost]
        public IActionResult Post([FromBody] Magic8BallResponseData data)
        {
            Magic8BallResponse newResponse = _service.Add(data);
            return Created(Request.GetEncodedUrl() + "/" + newResponse.Id, newResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _service.Delete(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
