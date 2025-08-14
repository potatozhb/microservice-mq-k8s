
using CommandsService.Models;
using CommandsService.Repos;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _appDbContext;
        public CommandRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public void CreateCommandForPlatform(int platformId, Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            command.PlatformID = platformId;
            this._appDbContext.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null) throw new ArgumentNullException(nameof(plat));
            this._appDbContext.Platforms.Add(plat);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return this._appDbContext.Platforms.Any(p => p.ExternalID == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return this._appDbContext.Platforms.ToList();
        }

        public Command GetCommandForPlatform(int platformId, int commandId)
        {
            return this._appDbContext.Commands
                .Where(c => c.PlatformID == platformId && c.ID == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return this._appDbContext.Commands
                .Where(c => c.PlatformID == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return this._appDbContext.Platforms.Any(p => p.ID == platformId);
        }

        public bool SaveChanges()
        {
            return this._appDbContext.SaveChanges() >= 0;
        }
    }
}