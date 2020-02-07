using System;

namespace Arup.AzureRequestListener
{
    public class LogChannel : ILogger
    {
        private Action<string> Logger { get; set; } = null;

        public LogChannel(Action<string> outputChannel)
        {
            Logger = outputChannel;
        }

        public void Log(string s)
        {
            Logger?.Invoke(s);
        }
    }
}