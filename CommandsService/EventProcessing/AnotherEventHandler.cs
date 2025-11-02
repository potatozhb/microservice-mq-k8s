using AutoMapper;

namespace CommandsService.EventProcessing
{
    public class AnotherEventHandler : IEventHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventType EventType => EventType.AnotherEvent;

        public AnotherEventHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void Handle(string message)
        {
            Console.WriteLine("--> Handling AnotherEvent event");
        }
    }
}
