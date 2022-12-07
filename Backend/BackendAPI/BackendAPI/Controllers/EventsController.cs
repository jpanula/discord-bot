using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_eventService.Get());
        }

        [HttpPost]
        public IActionResult PostEvent([FromBody] EventData data)
        {
            Event newEvent = _eventService.Add(data);
            return Created(Request.GetEncodedUrl() + "/" + newEvent.Id, newEvent);
        }

        [HttpPost("{id}")]
        public IActionResult PostEventVote(int id, [FromBody] EventVoteData data)
        {
            EventVote newVote = _eventService.AddVote(id, data);
            return Ok(newVote);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var deletedEvent = _eventService.Delete(id);
            if (deletedEvent == null)
            {
                return NotFound();
            }
            return Ok(deletedEvent);
        }
    }
}
