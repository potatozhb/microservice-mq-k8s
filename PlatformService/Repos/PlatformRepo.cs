
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Repos
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform? GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null) throw new ArgumentNullException(nameof(platform));
            _context.Platforms.Add(platform);
        }

        public void UpdatePlatform(Platform platform)
        {
            // No implementation needed for in-memory database
        }

        public void DeletePlatform(int id)
        {
            var platform = GetPlatformById(id);
            if (platform != null)
            {
                _context.Platforms.Remove(platform);
            }
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}