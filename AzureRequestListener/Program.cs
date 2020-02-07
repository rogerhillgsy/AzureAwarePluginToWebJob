using System;
using System.Collections.Generic;
using Arup.Common;
using Microsoft.Azure.WebJobs;

namespace Arup.AzureRequestListener
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var logger = new LogChannel(s => Console.WriteLine(s));
            var config = new JobHostConfiguration();
            var listenerConfig = new Config(logger);
            var webHostConfig = new JobHostConfiguration();
            MessageProcessingService.Logger = logger;
            logger.Log("Starting Arup SB Request Listener for CRM");

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }


            var listeners = new Dictionary<string, IMessageProcessor>
            {
                { 
                    OperationType.Ping,
                    new ProcessPing {Logger = logger, Config = listenerConfig}
                },
            };
            MessageProcessingService.MessageProcessors = listeners;

            // Kick off the Service Bus listener
            var listener = new TwoWayListener(webHostConfig, MessageProcessingService.Instance, listenerConfig, logger);
            listener.Start();



            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
