using CamundaInsurance.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages.Card
{
    [Authorize]
    public partial class InsuranceRequestComponent
    {
        [Inject]
        private ApplicationDbContext Context { get; set; }

       
    }
}
