using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Data.Models
{
    public class InsuranceRequest
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; }

        public string Reason { get; set; }

        public DateTime ApprovalTime { get; set; }

        [Required]
        public string InsuranceType { get; set; }

        [Required]
        public short  Height { get; set; }

        [Required]
        public short Weight { get; set; }

        [Required]       
        public short HealthHistoryRisk { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
