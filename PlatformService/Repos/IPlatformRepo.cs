
using PlatformService.Models;

namespace PlatformService.Repos
{
    public interface IPlatformRepo
    {
        IEnumerable<Platform> GetAllPlatforms();
        Platform? GetPlatformById(int id);
        void CreatePlatform(Platform platform);
        void UpdatePlatform(Platform platform);
        void DeletePlatform(int id);
        bool SaveChanges();
    }
}