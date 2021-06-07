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
    [HandlerTopics("insuranceDeny", LockDuration = 10000)]
    [HandlerVariables("requestId", "reason")]
    public class InsuranceDenyHandler : IExternalTaskHandler
    {
        private readonly InsuranceManager insuranceManager;

        public InsuranceDenyHandler(InsuranceManager insuranceManager)
        {
            this.insuranceManager = insuranceManager ?? throw new ArgumentNullException(nameof(insuranceManager));
        }

        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            if (!externalTask.Variables.TryGetValue("requestId", out var requestId))
            {
                Console.WriteLine("requestId is not provided");
                return new CompleteResult();
            }
            if (!externalTask.Variables.TryGetValue("reason", out var reason))
            {
                Console.WriteLine("reason is not found");
                return new CompleteResult();
            }
            var model = new InsuranceResponceModel
            {
                Id = requestId.AsString(),
                Reason = reason.AsString(),
                Status = InsuranceRequestStatus.Denied
            };
            await insuranceManager.HandleInsuranceResponce(model);
            return new CompleteResult();
        }
    }
}
