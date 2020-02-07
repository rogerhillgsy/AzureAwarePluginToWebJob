using Microsoft.Xrm.Sdk;

namespace Arup.AzureRequestListener
{
    public abstract class MessageProcessorBase : IMessageProcessor
    {
        public ILogger Logger { get; set; } = null;
       
        public IConfig Config { get; set; } = null;

        protected abstract Response ProcessSpecificMessage(RemoteExecutionContext context);

        public Response ProcessMessage(RemoteExecutionContext context)
        {
            Log($"Starting generic ProcessMessage");
            return ProcessSpecificMessage(context);
        }


        protected void Log(string s)
        {
            Logger?.Log(s);
        }
    }
}