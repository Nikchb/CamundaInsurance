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

        #region Request

        [Required]
        public string Triff { get; set; }

        [Required]
        public bool IsExistingCustomer { get; set; }

        [Required]
        public DateTime InsuranceStartDate { get; set; }

        [Required]
        public short Height { get; set; }

        [Required]
        public short Weight { get; set; }

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

        #endregion

        #region Responce

        public DateTime ApprovalDate { get; set; }

        public decimal Cost { get; set; }

        public string Reason { get; set; }

        public string ProcessId { get; set; }

        #endregion        

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }

    public static class InsuranceRequestStatus
    {
        public const string Approved = "Approved";
        public const string InProcess = "In Process";
        public const string Denied = "Denied";
        public const string None = "None";
    }
}
