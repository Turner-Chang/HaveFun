using HaveFun.Models;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.ViewModels
{
    public class ActivityViewModel
    {
        public Activity Activity { get; set; }

        public IEnumerable<ActivityType>? ActivityTypes { get; set; }

        //用IFormFile接受從Client傳來的圖片
        [Display(Name ="活動圖片")]
        public IFormFile? UploadedPicture { get; set; }
    }
}
