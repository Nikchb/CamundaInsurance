using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string SurName { get; set; }
        
        public string InsuranceCardNumber { get; set; }        

        [Required]
        public DateTime BirthDay { get; set; }
    }
}
