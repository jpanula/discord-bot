using BackendAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandService _commandService;

        public CommandsController(ICommandService commandService)
        {
            _commandService = commandService;
        }

        [HttpGet("CommandGroups")]
        public IActionResult GetCommandGroups()
        {
            return Ok(_commandService.GetCommandGroups());
        }

        [HttpGet("CommandGroups/{id}")]
        public IActionResult GetCommandGroupById(int id)
        {
            var commandGroup = _commandService.GetCommandGroupById(id);
            if (commandGroup == null)
            {
                return NotFound();
            }
            return Ok(commandGroup);
        }

        [HttpGet("SubCommands/{id}")]
        public IActionResult GetSubCommandById(int id)
        {
            var subCommand = _commandService.GetSubCommandById(id);
            if (subCommand == null)
            {
                return NotFound();
            }
            return Ok(subCommand);
        }
    }
}
