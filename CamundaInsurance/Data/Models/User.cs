using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [EmailAddress]
        public override string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SurName { get; set; }      
            
        [Required]
        public DateTime BirthDay { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$")]
        public string PostIndex { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        [NotMapped]
        public string Address => $"{PostIndex} {City}, {Street} {HouseNumber}";
    }
}
