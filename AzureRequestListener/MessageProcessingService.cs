using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace Arup.AzureRequestListener
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public sealed class MessageProcessingService : ITwoWayServiceEndpointPlugin
    {
        #region "Singleton implementation"

        private static Dictionary<string, IMessageProcessor> messageProcessors = null;

        private static readonly MessageProcessingService instance = new MessageProcessingService();

        static MessageProcessingService()
        {
        }

        private MessageProcessingService()
        {
        }

        public static MessageProcessingService Instance => instance;

        public static IJobInterface JobInterface { get; set; }

        #endregion

        public static Dictionary<string, IMessageProcessor> MessageProcessors { get; set; }

        public static ServiceHost Host { get; set; } = null;

        public static ILogger Logger { get; set; } = null;

        private static void Log(string s)
        {
            Logger?.Log(s);
        }

        public string Execute(RemoteExecutionContext executionContext)
        {
            Log($"\nMessage received at {DateTime.Now}");
            var rv = new Response { Message = "Unknown Error", Result = "0", Status = 500 };
            var operation = "Unknown";
            try
            {
                operation = (string)executionContext.SharedVariables["Operation"];
                Log($"processing Operation: {operation}");

                if (MessageProcessors.ContainsKey(operation))
                {
                    rv = MessageProcessors[operation].ProcessMessage(executionContext);
                }
                else
                {
                    Log($"Unknown operation request {operation}");
                    rv.Message = $"unknown operation: {operation}";
                    rv.Status = 501;
                }
            }
            catch (Exception ex)
            {
                Log($"Error processing operation {operation}");
                rv.Message = $"Error processing: {operation} {ex.Message}";
            }

            Log($"\nMessage processing completed received at {DateTime.Now}");
            return rv.ToJson();
        }
    }
}
