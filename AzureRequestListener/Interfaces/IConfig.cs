using System.Threading.Tasks;

namespace Arup.AzureRequestListener
{
    public interface IConfig
    {
        string ServiceBusSASKey{ get; }
        string SharedAccessKeyName { get; }
        string ServiceBusEndpoint { get; }

        Task<string> ServiceBusSASKeyTask { get; }

        ILogger Logger { get; }
    }
}
