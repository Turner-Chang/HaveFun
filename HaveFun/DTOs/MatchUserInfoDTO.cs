using HaveFun.Models;

namespace HaveFun.DTOs
{
	public class MatchUserInfoDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public int Gender { get; set; }
		public DateTime BirthDay { get; set; }

		public int Age { get; set; }
		public string ProfilePicture { get; set; }
		public string Introduction { get; set; }
		public int Level { get; set; }
		public ICollection<MatchLabelDTO> Labels { get; set; }
		public ICollection<UserPicture> Pictures { get; set; }
	}
}
