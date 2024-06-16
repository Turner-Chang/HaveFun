using Microsoft.AspNetCore.Mvc;

namespace HaveFun.DTOs
{
    public class WhoLikeListDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
    }
}
