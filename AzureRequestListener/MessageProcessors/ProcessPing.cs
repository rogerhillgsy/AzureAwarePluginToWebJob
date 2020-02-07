using Arup.Common;
using Microsoft.Xrm.Sdk;

namespace Arup.AzureRequestListener
{
    public class ProcessPing : MessageProcessorBase, IMessageProcessor
    {

        protected override Response ProcessSpecificMessage(RemoteExecutionContext context)
        {
            Log($"Starting processing Ping request");
            var value = context.SharedVariables[SharedVariables.PingVal] as string;

            Log($"Ping value {value}");

            return new Response { Result = value };
        }
    }
}