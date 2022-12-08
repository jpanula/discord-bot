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
        public IActionResult GetEvent()
        {
            return Ok(_eventService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            var selectedEvent = _eventService.GetById(id);
            if (selectedEvent == null)
            {
                return NotFound();
            }
            return Ok(selectedEvent);
        }

        [HttpGet("{id}/Votes")]
        public IActionResult GetEventVotes(int id)
        {
            var selectedEvent = _eventService.GetById(id);
            if (selectedEvent == null)
            {
                return NotFound();
            }
            return Ok(_eventService.GetVotes(id));
        }

        [HttpPost]
        public IActionResult PostEvent([FromBody] EventData data)
        {
            Event newEvent = _eventService.Add(data);
            return Created(Request.GetEncodedUrl() + "/" + newEvent.Id, newEvent);
        }

        [HttpPost("{id}/Votes")]
        public IActionResult PostEventVote(int id, [FromBody] EventVoteData data)
        {
            EventVote newVote = _eventService.AddVote(id, data);
            return Ok(newVote);
        }

        [HttpPost("{id}/Messages")]
        public IActionResult PostMessageId(int id, string messageId)
        {
            Event updatedEvent = _eventService.AddMessageId(id, messageId);
            if (updatedEvent == null)
            {
                return NotFound();
            }
            return Ok(updatedEvent);
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

        [HttpDelete("{id}/Votes")]
        public IActionResult DeleteEventVote(int id, [FromBody] EventVoteData data)
        {
            var deletedEventVote = _eventService.DeleteVote(id, data);
            if (deletedEventVote == null )
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
