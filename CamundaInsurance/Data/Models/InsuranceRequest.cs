﻿using System;
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
        public string InsuranceType { get; set; }

        [Required]
        public short Height { get; set; }

        [Required]
        public short Weight { get; set; }

        [Required]
        public short HealthHistoryRisk { get; set; }

        #endregion

        #region Responce

        public DateTime ApprovalTime { get; set; }

        public decimal Cost { get; set; }

        public string Reason { get; set; }

        #endregion

        #region Calculated
        [NotMapped]
        public decimal BMI => Weight / (decimal)Math.Pow(((double)Height) / 100, 2);
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
