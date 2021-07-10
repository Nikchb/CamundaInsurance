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
        public bool IsExistingCustomer { get; set; }

        [Required]        
        public short Height { get; set; }

        [Required]
        public short Weight { get; set; }

        [Required]
        public DateTime InsuranceStartDate { get; set; } = DateTime.Now + TimeSpan.FromDays(7);

        [Required]
        public bool Disease1RC1 { get; set; }

        [Required]
        public bool Disease2RC1 { get; set; }

        [Required]
        public bool Disease3RC1 { get; set; }

        [Required]
        public bool Disease1RC2 { get; set; }

        [Required]
        public bool Disease2RC2 { get; set; }

        [Required]
        public bool Disease3RC2 { get; set; }

        [Required]
        public bool Disease1RC3 { get; set; }

        [Required]
        public bool Disease2RC3 { get; set; }

        [Required]
        public bool Disease3RC3 { get; set; }
    }
}
