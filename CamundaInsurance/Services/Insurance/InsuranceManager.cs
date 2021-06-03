using CamundaInsurance.Data;
using CamundaInsurance.Data.Models;
using CamundaInsurance.Services.Insurance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services.Insurance
{
    public class InsuranceManager : ServiceBase
    {
        private readonly ApplicationDbContext context;

        private readonly IdentityService identityService;

        public InsuranceManager(ApplicationDbContext context, IdentityService identityService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
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
                ApprovalTime = lastRequest.ApprovalTime,
                InsuranceType = lastRequest.InsuranceType,
                Reason = lastRequest.Reason                
            });
        }  
        
        public async Task<SResponce> SendInsuranceRequest(InsuranceRequestModel model)
        {
            var result = Tools.ValidateModel(model);
            if(result != null)
            {
                return Error(result.Select(v=>v.ErrorMessage).ToArray());
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

            var insuranceRequest = new InsuranceRequest 
            {
                Status = InsuranceRequestStatus.InProcess,
                InsuranceType = model.InsuranceType,
                Height = model.Height,
                Weight = model.Weight,
                HealthHistoryRisk = model.HealthHistoryRisk,
                UserId = user.Id
            };

            // send request to camunda
            var camundaResponce = new SContentResponce<int>(true);
            if(camundaResponce.Succeeded == false)
            {
                return Error("Service in temporary unavalable");
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

            await context.SaveChangesAsync();

            return Ok();
        }

        //public async Task<SResponce> Re
    }
}
