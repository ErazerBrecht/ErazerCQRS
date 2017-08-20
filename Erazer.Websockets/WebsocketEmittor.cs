using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Erazer.Services.Events;
using Erazer.Services.Events.Mappings;
using Erazer.Web.Shared;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace Erazer.Websockets
{
    public class WebsocketEmittor : IWebsocketEmittor
    {
        // TODO DI HttpClient!?
        // Use Singleton => https://softwareengineering.stackexchange.com/questions/330364/correct-way-of-using-httpclient
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly IOptions<WebsocketSettings> _settings;

        public WebsocketEmittor(IOptions<WebsocketSettings> settings)
        {
            _settings = settings;
        }

        public async Task Emit(ReduxAction action)
        {
            var jsonString = JsonConvert.SerializeObject(action, JsonSettings.CamelCaseSerializer);

            await HttpClient.PostAsync(_settings.Value.ConnectionString, new StringContent(jsonString, Encoding.UTF8, "application/json"));
            // TODO THROW ERROR WHEN POST FAILED
        }
    }
}
