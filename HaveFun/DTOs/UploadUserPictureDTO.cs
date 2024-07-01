namespace HaveFun.DTOs
{
	public class UploadUserPictureDTO
	{
		public int UserId { get; set; }
		public IFormFile File { get; set; }
	}
}
