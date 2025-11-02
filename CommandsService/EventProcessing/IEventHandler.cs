
namespace CommandsService.EventProcessing
{
    public interface IEventHandler
    {
        EventType EventType { get; }
        void Handle(string message);
    }
}