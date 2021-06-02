using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Models
{
    public class InsuranceRequestModel
    {
        [Required]
        public string InsuranceType { get; set; } = "Premium";

        [Required]        
        public short Height { get; set; }

        [Required]
        public short Weight { get; set; }

        [Required]
        public short HealthHistoryRisk { get; set; } = 0;
    }
}
