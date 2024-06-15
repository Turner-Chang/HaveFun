using HaveFun.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.Areas.ManagementSystem.DTOs
{
    public class ChangePostStateDTO
    {
        public int Id { get; set; }


        public int Status { get; set; }


    }
}