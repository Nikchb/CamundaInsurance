using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services.Insurance.Models
{
    public class InsuranceResponceModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Status { get; set; }

        public decimal Cost { get; set; }

        public string Reason { get; set; }
    }
}
