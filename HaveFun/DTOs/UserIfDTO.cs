using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.DTOs
{
    public class UserIfDTO
    {
        public int Id { get; set; }
        // 暱稱        
        [StringLength(maximumLength: 50, ErrorMessage = "暱稱長度不能超過50字")]
        public string? Name { get; set; }

        // 地址
        [StringLength(maximumLength: 100, ErrorMessage = "地址長度不可超過100字")]
        public string? Address { get; set; }

        // 電話
        [StringLength(maximumLength: 20, MinimumLength = 10, ErrorMessage = "電話號碼應該介於 10 到 20 個字元之間")]
        [RegularExpression("^[\\d\\+\\-\\(\\) ]+$", ErrorMessage = "電話號碼格式不正確")]
        public string? PhoneNumber { get; set; }

        // 性別
        [Required(ErrorMessage = "請輸入性別")]
        [Range(0, 1, ErrorMessage = "請輸入正確的性別")]
        public int Gender { get; set; }

        // 生日
        [Required(ErrorMessage = "請輸入出生年月日")]
        public DateTime BirthDay { get; set; }

        //圖片
        public IFormFile? ProfilePicture { get; set; }
        
        public string? Introduction { get; set; }

		public List<IFormFile>? AlbumPictures { get; set; }

		public int Level { get; set; } = 0;
	}
}

