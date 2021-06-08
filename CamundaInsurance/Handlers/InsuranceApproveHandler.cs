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
    [HandlerTopics("insuranceApprove", LockDuration = 10000)]
    [HandlerVariables("requestId", "rate")]
    public class InsuranceApproveHandler : IExternalTaskHandler
    {
        private readonly InsuranceManager insuranceManager;

        public InsuranceApproveHandler(InsuranceManager insuranceManager)
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
            if (!externalTask.Variables.TryGetValue("rate", out var rate))
            {
                Console.WriteLine("rate is not found");
                return new CompleteResult();
            }
            var model = new InsuranceResponceModel
            {
                Id = requestId.AsString(),
                Cost = rate.AsInteger(),
                Status = InsuranceRequestStatus.Approved
            };
            await insuranceManager.HandleInsuranceResponce(model);
            return new CompleteResult();
        }
    }
}
