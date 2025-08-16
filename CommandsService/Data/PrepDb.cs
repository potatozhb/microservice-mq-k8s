using CommandsService.Models;
using CommandsService.Repos;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder builder)
        {
            using (var service = builder.ApplicationServices.CreateScope())
            {
                var grpcClient = service.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();
                SeedData(service.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platform...");
            foreach (var plat in platforms)
            {
                if (!repo.ExternalPlatformExists(plat.ExternalID))
                {
                    repo.CreatePlatform(plat);
                }
                repo.SaveChanges();
            }
        }
    }
}