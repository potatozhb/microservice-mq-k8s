using AutoMapper;
using CommandService.Dtos;
using CommandsService.Models;
using CommandsService.Repos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController: ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repo, IMapper mapper) 
        {
            _repo= repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform:{platformId}");
            if (!this._repo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = this._repo.GetCommandsForPlatform(platformId);
            return Ok(this._mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform:{platformId} , {commandId}");
            if (!this._repo.PlatformExists(platformId))
            {
                return NotFound();
            }
            
            var command = this._repo.GetCommandForPlatform(platformId, commandId);
            if(command == null)
            {
                return NotFound();
            }

            return Ok(this._mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId,
            CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform:{platformId}");
            if (!this._repo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = this._mapper.Map<Command>(commandDto);
            this._repo.CreateCommandForPlatform(platformId, command);
            this._repo.SaveChanges();

            var commandReadDto = this._mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), 
                new {PlatformID =  platformId, CommandID = commandReadDto.ID}, commandReadDto);
        }
    }
}