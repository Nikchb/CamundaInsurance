using CamundaInsurance.Data;
using CamundaInsurance.Data.Models;

using CamundaInsurance.Services.Insurance.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camunda.Api.Client;
using Camunda.Api.Client.Message;

namespace CamundaInsurance.Services.Insurance
{
    public class InsuranceManager : ServiceBase
    {
        private readonly ApplicationDbContext context;

        private readonly IdentityService identityService;

        private readonly CamundaClient camundaClient;

        public InsuranceManager(ApplicationDbContext context, IdentityService identityService, CamundaClient camundaClient)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.camundaClient = camundaClient ?? throw new ArgumentNullException(nameof(camundaClient));
        }

        public async Task<SContentResponce<InsuranceInfoModel>> GetInsuranceInfoAsync()
        {
            var identityResponce = await identityService.GetCurrentUserAsync();
            if (identityResponce.Succeeded == false)
            {
                return Error<InsuranceInfoModel>(identityResponce.Messages);
            }
            var user = identityResponce.Content;

            var userRequest = context.InsuranceRequests.Where(v => v.UserId == user.Id);
            if(userRequest.Count() == 0)
            {
                return Ok(new InsuranceInfoModel
                {
                    Status = InsuranceRequestStatus.None
                });
            }
            var lastRequest = userRequest.OrderBy(v => v.CreationTime).Last();
            return Ok(new InsuranceInfoModel
            {
                Status = lastRequest.Status,                
                Cost = lastRequest.Cost,
                ApprovalDate = lastRequest.ApprovalDate,
                Tariff = lastRequest.Triff,
                Reason = lastRequest.Reason,
                InsuranceStartDate = lastRequest.InsuranceStartDate
            });
        }  
        
        public async Task<SResponce> SendInsuranceRequest(InsuranceRequestModel model)
        {
            var result = Tools.ValidateModel(model);
            if(result != null)
            {
                return Error(result.Select(v=>v.ErrorMessage).ToArray());
            }

            if(model.InsuranceStartDate < DateTime.Now.Date + TimeSpan.FromDays(7))
            {
                return Error("Insurance start date should be at least 7 days after the request");
            }

            var identityResponce = await identityService.GetCurrentUserAsync();
            if(identityResponce.Succeeded == false)
            {
                return Error(identityResponce.Messages);
            }
            var user = identityResponce.Content;

            if (context.InsuranceRequests.Where(v=>v.UserId == user.Id).Any(v=>v.Status == InsuranceRequestStatus.Approved))
            {
                return Error("Insurance is already approved, deactivate old insurance before requestung new one");
            }

            if (context.InsuranceRequests.Where(v => v.UserId == user.Id).Any(v => v.Status == InsuranceRequestStatus.InProcess))
            {
                return Error("Insurance request is already in process, please wait for the result");
            }

            // it will be better to use mapper...
            var insuranceRequest = new InsuranceRequest
            {
                Status = InsuranceRequestStatus.InProcess,
                Triff = model.Tariff,
                IsExistingCustomer = model.IsExistingCustomer,
                InsuranceStartDate = model.InsuranceStartDate,
                Height = model.Height,
                Weight = model.Weight,
                Disease1RC1 = model.Disease1RC1,
                Disease1RC2 = model.Disease1RC2,
                Disease1RC3 = model.Disease1RC3,
                Disease2RC1 = model.Disease2RC1,
                Disease2RC2 = model.Disease2RC2,
                Disease2RC3 = model.Disease2RC3,
                Disease3RC1 = model.Disease3RC1,
                Disease3RC2 = model.Disease3RC2,
                Disease3RC3 = model.Disease3RC3,
                UserId = user.Id
            };


            var message = new CorrelationMessage() 
            { 
                All = true, 
                MessageName = "InsuranceApplicationForm"
            };
            message.ProcessVariables
                .Set("requestId", insuranceRequest.Id)
                .Set("name", user.Name)
                .Set("surname", user.SurName)
                .Set("birthDate", user.BirthDay)
                .Set("address", user.Address)
                .Set("gender", user.Gender)
                .Set("tariff", insuranceRequest.Triff)
                .Set("insuranceStartDate", insuranceRequest.InsuranceStartDate)
                .Set("height", insuranceRequest.Height)
                .Set("weight", insuranceRequest.Weight)
                .Set("disease1RC1", insuranceRequest.Disease1RC1)
                .Set("disease2RC1", insuranceRequest.Disease2RC1)
                .Set("disease3RC1", insuranceRequest.Disease3RC1)
                .Set("disease1RC2", insuranceRequest.Disease1RC2)
                .Set("disease2RC2", insuranceRequest.Disease2RC2)
                .Set("disease3RC2", insuranceRequest.Disease3RC2)
                .Set("disease1RC3", insuranceRequest.Disease1RC3)
                .Set("disease2RC3", insuranceRequest.Disease2RC3)
                .Set("disease3RC3", insuranceRequest.Disease3RC3)
                .Set("isExistingCustomer", insuranceRequest.IsExistingCustomer);
            try
            {
                var camundaResponce = await camundaClient.Messages.DeliverMessage(message);
            }
            catch
            {
                return Error("Service temporarily unavailable");
            }                    
            
            await context.InsuranceRequests.AddAsync(insuranceRequest);
            await context.SaveChangesAsync();
            return Ok();
        }

        public async Task<SResponce> HandleInsuranceResponce(InsuranceResponceModel model)
        {
            var result = Tools.ValidateModel(model);
            if (result != null)
            {
                return Error(result.Select(v => v.ErrorMessage).ToArray());
            }

            var insuranceRequest = await context.InsuranceRequests.FindAsync(model.Id);
            if(insuranceRequest == null)
            {
                return Error("Insurance request not found");
            }

            insuranceRequest.Status = model.Status;
            insuranceRequest.Cost = model.Cost;
            insuranceRequest.Reason = model.Reason;
            insuranceRequest.ApprovalDate = DateTime.Now;
            insuranceRequest.ProcessId = model.ProcessId;

            await context.SaveChangesAsync();

            return Ok();
        }

        public async Task<SResponce> RevokeInsurance()
        {
            var identityResponce = await identityService.GetCurrentUserAsync();
            if (identityResponce.Succeeded == false)
            {
                return Error(identityResponce.Messages);
            }
            var user = identityResponce.Content;

            var request = context.InsuranceRequests
                .Where(v => v.UserId == user.Id && v.Status == InsuranceRequestStatus.Approved)
                .OrderBy(v => v.CreationTime)
                .LastOrDefault();
            if (request == null)
            {
                return Error("No active insurance found");
            }

            if(DateTime.Now.Date - request.ApprovalDate.Date > TimeSpan.FromDays(14))
            {
                return Error("Insurance can not be revoked after 14 days");
            }

            var message = new CorrelationMessage()
            {
               ProcessInstanceId = request.ProcessId,
               MessageName = "ReceiptOfCancellation"
            };

            try
            {
                var camundaResponce = await camundaClient.Messages.DeliverMessage(message);
            }
            catch(Exception e)
            {
                return Error("Service temporarily unavailable", e.Message);
            }

            request.Status = InsuranceRequestStatus.Denied;
            request.Reason = "Insurance is revoked by the user";

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
