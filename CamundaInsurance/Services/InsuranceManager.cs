using CamundaInsurance.Data;
using CamundaInsurance.Data.Models;
using CamundaInsurance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services
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
            var currentUser = await identityService.GetCurrentUserAsync();
            var userRequest = context.InsuranceRequests.Where(v => v.UserId == currentUser.Id);
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

            var user = await identityService.GetCurrentUserAsync();            

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
    }
}
