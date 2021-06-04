using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services.Insurance.Models
{
    public class InsuranceInfoModel
    {        
        public string Status { get; set; }
        public string Tariff { get; set; }
        public decimal Cost { get; set; }
        public string Reason { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime InsuranceStartDate { get; set; }
        public bool CanDeactivate => DateTime.Now.Date < DeactivationDeadline;
        public DateTime DeactivationDeadline => ApprovalDate.Date.Date + TimeSpan.FromDays(14);
    }
}
