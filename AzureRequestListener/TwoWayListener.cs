using System;
using System.ServiceModel;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus;
using Microsoft.Xrm.Sdk;

namespace Arup.AzureRequestListener
{
    public class TwoWayListener
    {
        private JobHostConfiguration _config = null;
        private IJobInterface _jobInterface = null;

        private IConfig _listenerConfig;

        private ITwoWayServiceEndpointPlugin _messageProcessor;

        #region Logging
        public static ILogger Logger { get; private set; }
        private static void Log(string s)
        {
            Logger?.Log(s);
        }
        #endregion  
        public TwoWayListener(JobHostConfiguration config, ITwoWayServiceEndpointPlugin messageProcessor, IConfig listenerConfig, ILogger logger = null)
        {
            _config = config;
            if (logger != null)
            {
                Logger = logger;
            }

            _listenerConfig = listenerConfig;
            _messageProcessor = messageProcessor;

        }

        /// <summary>
        /// Start the two way listener running as a service host, listening for requests on the Azure Service Bus.
        /// </summary>
        public void Start()
        {
            try
            {
                var SBaccessKey = _listenerConfig.ServiceBusSASKey;
                var serviceHost = new ServiceHost(_messageProcessor);
                MessageProcessingService.Host = serviceHost;

                Log($"Starting TwoWay Listener on {_listenerConfig.ServiceBusEndpoint} with access policy {_listenerConfig.SharedAccessKeyName}");

                var tokenProvider =
                    TokenProvider.CreateSharedAccessSignatureTokenProvider(_listenerConfig.SharedAccessKeyName,
                        SBaccessKey);
                // accessKeyTask.Result);
                var transportClient = new TransportClientEndpointBehavior(tokenProvider);

                serviceHost.AddServiceEndpoint(typeof(ITwoWayServiceEndpointPlugin), 
                    new WS2007HttpRelayBinding(),
                    _listenerConfig.ServiceBusEndpoint).Behaviors.Add(transportClient);

                serviceHost.Open();

                Log($"listening ... on {serviceHost.Description.Endpoints[0].Address}");
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                throw;
            }
        }
    }
}