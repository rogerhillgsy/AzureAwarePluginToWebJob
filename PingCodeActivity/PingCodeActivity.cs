using System.Activities;
using System.Linq;
using Arup.Common;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace Arup.AzureCodeActivity
{
    /// <summary>
    /// Base class for all code activities that interact with the Azure Oracle Request Listener
    /// </summary>
    public class PingCodeActivity : CodeActivity
    {

        /// <summary>
        /// Enables the service endpoint to be provided when this activity is added as a 
        /// step in a workflow.
        /// </summary>
        [RequiredArgument]
        [ReferenceTarget("serviceendpoint")]
        [Input("Service Bus Endpoint for Oracle Request Listener")]
        public InArgument<EntityReference> ServiceEndpoint { get; set; }

        //Input Parameter - Curent CJN
        [Input("Ping Value")]
        public InArgument<string> PingValue { get; set; }

        protected IWorkflowContext Context;
        protected IOrganizationServiceFactory ServiceFactory;
        protected IOrganizationService Service;
        protected ITracingService Trace;

        protected override void Execute(CodeActivityContext executionContext)
        {
            Context = executionContext.GetExtension<IWorkflowContext>();
            ServiceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            Service = ServiceFactory.CreateOrganizationService(Context.UserId);
            Trace = executionContext.GetExtension<ITracingService>();
            Trace.Trace($"Starting Ping");

            //retrieve configuration values 
            var configurationRequest = new OrganizationRequest("ccrm_GetConfigurationValues");
            var configurationResponse = Service.Execute(configurationRequest);

            //define System Name Variable 
            var crmSystemName = string.Empty;
            if (configurationResponse.Results.Contains("CrmSystem"))
                crmSystemName = (string)configurationResponse.Results["CrmSystem"];
            Trace.Trace($"System Name is {crmSystemName}");
            Context.SharedVariables.Add(SharedVariables.CRMSystemName, crmSystemName);

            Context.SharedVariables.Add(SharedVariables.Operation, OperationType.Ping);
            var pingValue = PingValue.Get(executionContext);
            if (string.IsNullOrEmpty(pingValue))
            {
                pingValue = "1234";
            }
            Context.SharedVariables.Add(SharedVariables.PingVal, pingValue);

            Trace.Trace(
                $"Pinging Oracle Request Listener: {string.Join(", ", (from s in Context.SharedVariables select $"{s.Key}:{s.Value}"))}]");
            Trace.Trace($"Calling service endpoint for Ping");
            IServiceEndpointNotificationService endpointService =
                executionContext.GetExtension<IServiceEndpointNotificationService>();

            var result = endpointService.Execute(ServiceEndpoint.Get(executionContext), Context);

            Trace.Trace($"Response received {result}");

            // Pass the result in shared variable as a work around for the issue
            // with calling the service endpoint from the Opportunity
            Context.SharedVariables.Add(SharedVariables.Result, result);

        }
    }
}
