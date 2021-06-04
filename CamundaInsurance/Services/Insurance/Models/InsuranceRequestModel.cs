using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services.Insurance.Models
{
    public class InsuranceRequestModel
    {
        [Required]
        public string Tariff { get; set; } = "Premium";

        [Required]        
        public short Height { get; set; }

        [Required]
        public short Weight { get; set; }

        [Required]
        public DateTime InsuranceStartDate { get; set; } = DateTime.Now + TimeSpan.FromDays(7);
        
        public string PreExistingConditions { get; set; }
    }
}
