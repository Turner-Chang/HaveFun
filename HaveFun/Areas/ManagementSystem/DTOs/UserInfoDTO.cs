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
		public int AccountStatus { get; set; }
		public int Level { get; set; }
		public DateTime? RegistrationDate { get; set; }
		public string? ProfilePicture { get; set; }
		public string? Address { get; set; }
		public string? PhoneNumber { get; set; }
		public string Account { get; set; }
	}
}