using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Models
{
    public class InsuranceInfoModel
    {        
        public string Status { get; set; }
        public string InsuranceType { get; set; }
        public decimal Cost { get; set; }
        public string Reason { get; set; }
        public DateTime ApprovalTime { get; set; }
        public bool CanDeactivate => DateTime.Now.Date > DeactivationDeadline;
        public DateTime DeactivationDeadline => ApprovalTime.Date.Date + TimeSpan.FromDays(14);
    }
}
