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
    [HandlerTopics("instruction", LockDuration = 10000)]
    [HandlerVariables("processId", "requestId", "reason")]
    public class InstructionHandler : IExternalTaskHandler
    {
        private readonly InsuranceManager insuranceManager;

        private readonly CamundaClient camundaClient;

        public InstructionHandler(InsuranceManager insuranceManager, CamundaClient camundaClient)
        {
            this.insuranceManager = insuranceManager ?? throw new ArgumentNullException(nameof(insuranceManager));
            this.camundaClient = camundaClient ?? throw new ArgumentNullException(nameof(camundaClient));
        }

        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {           
            if (!externalTask.Variables.TryGetValue("requestId", out var requestId))
            {
                Console.WriteLine("requestId is not provided");
                return new CompleteResult();
            }

            string instruction;
            if (externalTask.Variables.TryGetValue("reason", out var reason))
            {
                instruction = reason.AsString();
            }
            else
            {
                instruction = "Instructions not provided, please contact support";
            }        
          
            var model = new InsuranceResponceModel
            {
                Id = requestId.AsString(),
                Reason = instruction,
                Status = InsuranceRequestStatus.Denied
            };
            await insuranceManager.HandleInsuranceResponce(model);
            return new CompleteResult();
        }
    }
}
