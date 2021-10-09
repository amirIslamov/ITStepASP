using System.ComponentModel.DataAnnotations;

namespace ITStepASP.Auth.Dto
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(4)]
        [MaxLength(16)]
        public string UserName { get; set; }
        
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        
        [StringLength(8)]
        [MaxLength(16)]
        [Required]
        public string Password { get; set; }
        
        [StringLength(8)]
        [MaxLength(16)]
        [Required]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
    }
}