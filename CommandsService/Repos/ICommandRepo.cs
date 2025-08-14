using CommandsService.Models;

namespace CommandsService.Repos
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);

        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommandForPlatform(int platformId, int commandId);
        void CreateCommandForPlatform(int platformId, Command command);

    }
}