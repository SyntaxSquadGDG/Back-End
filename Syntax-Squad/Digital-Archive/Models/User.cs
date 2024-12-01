using Microsoft.AspNetCore.Identity;

namespace Digital_Archive.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string hashed_Password { get; set; }
    }
}
