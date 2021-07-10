using Camunda.Api.Client;
using Camunda.Api.Client.Message;
using Camunda.Worker;
using CamundaInsurance.Data.Models;
using CamundaInsurance.Services.Insurance;
using CamundaInsurance.Services.Insurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CamundaInsurance.Handlers
{
    [HandlerTopics("approval", LockDuration = 10000)]
    [HandlerVariables("processId", "rate", "requestId")]
    public class ApprovalHandler : IExternalTaskHandler
    {
        private readonly InsuranceManager insuranceManager;

        private readonly CamundaClient camundaClient;

        public ApprovalHandler(InsuranceManager insuranceManager, CamundaClient camundaClient)
        {
            this.insuranceManager = insuranceManager ?? throw new ArgumentNullException(nameof(insuranceManager));
            this.camundaClient = camundaClient ?? throw new ArgumentNullException(nameof(camundaClient));
        }

        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            if (externalTask.Variables.TryGetValue("processId", out var processId))
            {
                var message = new CorrelationMessage()
                {
                    ProcessInstanceId = processId.AsString(),
                    MessageName = "Approval"
                };
                foreach (var varible in externalTask.Variables)
                {                   
                    message.SetVariable(varible.Key, varible.Value.Value);
                }
                message.SetVariable("processId", externalTask.ProcessInstanceId);
                try
                {
                    var camundaResponce = await camundaClient.Messages.DeliverMessage(message);
                }
                catch(Exception e)
                {
                    return new FailureResult(e.Message);
                }
            }

            if (!externalTask.Variables.TryGetValue("requestId", out var requestId))
            {
                Console.WriteLine("requestId is not provided");
                return new CompleteResult();
            }
            if (!externalTask.Variables.TryGetValue("rate", out var rate))
            {
                return new FailureResult("Insurance rate is not provided");
            }
            var model = new InsuranceResponceModel
            {
                Id = requestId.AsString(),
                Cost = rate.AsLong(),                
                Status = InsuranceRequestStatus.Approved,
                ProcessId = externalTask.ProcessInstanceId
            };
            await insuranceManager.HandleInsuranceResponce(model);
            return new CompleteResult();
        }
    }
}
