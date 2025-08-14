
using PlatformService.Dtos;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to Command Service was successful");
            }
            else
            {
                Console.WriteLine("--> Sync POST to Command Service failed");
            }
        }


        public async Task SendGetPlatformToCommand(IEnumerable<PlatformReadDto> platforms)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platforms),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.GetAsync($"{_configuration["CommandService"]}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync GET to Command Service was successful");
            }
            else
            {
                Console.WriteLine("--> Sync GET to Command Service failed");
            }
        }
    }
}