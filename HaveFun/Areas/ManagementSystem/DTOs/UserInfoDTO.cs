namespace HaveFun.Areas.ManagementSystem.DTOs
{
    public class UserInfoDTO
    {

		public int Id { get; set; }
		public int Status { get; set; }
		public string? Name { get; set; }
		public int? Gender { get; set; }
		public string? Introduction { get; set; }
		public DateTime? BirthDay { get; set; }
	}
}