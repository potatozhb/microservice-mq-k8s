
using System.Text.Json;
using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using CommandsService.Repos;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly Dictionary<EventType, IEventHandler> _handlers;

        public EventProcessor(IEnumerable<IEventHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.EventType, h => h);
        }

        public void ProcessEvent(string message)
        {
            Console.WriteLine($"--> Processing event: {message}");
            var eventType = DetermineEvent(message);
            
            if(_handlers.TryGetValue(eventType, out var handler))
            {
                handler.Handle(message);
            }
            else
            {
                Console.WriteLine($"--> No handler registered for {eventType}");
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine($"--> Determining Event : {notificationMessage}");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the type Event");
                    return EventType.Undetermined;
            }
        }
    }

}