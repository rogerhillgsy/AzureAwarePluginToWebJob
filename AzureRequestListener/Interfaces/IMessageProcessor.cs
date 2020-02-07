using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Arup.AzureRequestListener
{
    public interface IMessageProcessor
    {
        ILogger Logger { get; set; }

        Response ProcessMessage(RemoteExecutionContext context);
    }
}
