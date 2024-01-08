using System.ComponentModel.DataAnnotations;

namespace P140_Pronia.ViewModels
{
    public class LoginVM
    {
        [Required, StringLength(10, MinimumLength = 3), Display(Prompt = "Username")]
        public string Username { get; set; } = null!;
        [Display(Prompt = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Display(Name = "Remember me",Prompt = "Username")]
        public bool RememberMe { get; set; }
    }
}
