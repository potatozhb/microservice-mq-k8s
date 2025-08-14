using AutoMapper;
using CommandService.Dtos;
using CommandsService.Repos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        // This controller will handle requests related to platforms.
        // You can add methods here to manage platform data, such as getting a list of platforms,
        // adding a new platform, updating an existing platform, or deleting a platform.

        public PlatformsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            // Constructor logic can be added here if needed
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Platforms Controller");
            return Ok("Inbound test from Platforms Controller");
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            Console.WriteLine("--> Inbound GET # Platforms Controller");
            
            var platforms = this._repo.GetAllPlatforms();

            return Ok(this._mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }
    }
}