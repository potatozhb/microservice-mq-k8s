using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using CommandsService.Repos;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    public class PlatformPublishedHandler : IEventHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventType EventType => EventType.PlatformPublished;

        public PlatformPublishedHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        //use async in future
        public void Handle(string message)
        {
            Console.WriteLine("--> Handling PlatformPublished event");
            PlatformPublishDto? dto;
            try
            {
                dto = JsonSerializer.Deserialize<PlatformPublishDto>(message);
                if(dto == null)
                {
                    Console.WriteLine("--> Invalid PlatformPublished payload");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Deserialization failed: {ex.Message}");
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            try
            {
                var platform = _mapper.Map<Platform>(dto);

                if (!repo.ExternalPlatformExists(platform.ExternalID))
                {
                    repo.CreatePlatform(platform);
                    repo.SaveChanges();
                    Console.WriteLine("--> Platform added");
                }
                else
                {
                    Console.WriteLine("--> Platform already exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not save platform: {ex.Message}");
            }
        }
    }
}