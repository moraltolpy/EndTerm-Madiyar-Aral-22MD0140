using System.ComponentModel.DataAnnotations;

namespace EndTerm.Models
{
    public class User: UserDto
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;

    }
    public class UserDto
    {
        [Key]
        public string Login { get; set; } = null!;
        public string Password { get; set; }
    }
}
