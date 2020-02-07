using System.Configuration;
using System.Threading.Tasks;

namespace Arup.AzureRequestListener
{
     public class Config : IConfig
    {
        public Config(ILogger logger)
        {
            Logger = logger;
        }

        public string ServiceBusSASKey => ConfigurationManager.AppSettings.Get("ServiceBusSASKey");

        public string SharedAccessKeyName =>  ConfigurationManager.AppSettings.Get("SharedAccessKeyName"); 

        public string ServiceBusEndpoint =>  ConfigurationManager.AppSettings.Get("ServiceBusEndpoint"); 

        public Task<string> ServiceBusSASKeyTask { get; }
        public Task<string> SharedAccessKeyNameTask { get; }
        public Task<string> ServiceBusEndpointTask { get; }
        public ILogger Logger { get; }
    }
}
