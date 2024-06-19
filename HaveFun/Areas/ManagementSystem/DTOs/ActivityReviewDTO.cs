using HaveFun.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.Areas.ManagementSystem.DTOs
{
    public class ActivityReviewDTO
    {
        public int ActivityReviewId { get; set; }


        public int ActivityId { get; set; }


        public int UserId { get; set; }



        public int ReportItems { get; set; }




        public string ReportReason { get; set; }


        public DateTime ReportTime { get; set; }


        public int ProcessingStstus { get; set; } = 0;

    }
}