using System.ComponentModel.DataAnnotations;

namespace SDSYSTEM.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Administrator,
        Coordinator,
        Support,
        User
    }
}
