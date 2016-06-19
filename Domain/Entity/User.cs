using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class User
    {
        [Key]
        [Required]
        public string Login { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public bool IsBlocked { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
