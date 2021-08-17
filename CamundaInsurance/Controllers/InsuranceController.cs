using CamundaInsurance.Services.Insurance;
using CamundaInsurance.Services.Insurance.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Controllers
{
    [Route("api/insurance")]
    public class InsuranceController : ControllerBase
    {
        private readonly InsuranceManager insuranceManager;

        public InsuranceController(InsuranceManager insuranceManager)
        {
            this.insuranceManager = insuranceManager ?? throw new ArgumentNullException(nameof(insuranceManager));
        }

        [HttpPut]        
        public async Task<IActionResult> UpdateInsurance([FromBody] InsuranceResponceModel model)
        {
            var responce = await insuranceManager.HandleInsuranceResponce(model);
            if(responce.Succeeded)
            {
                return Ok();
            }
            return BadRequest(responce.Messages);
        }
    }
}
