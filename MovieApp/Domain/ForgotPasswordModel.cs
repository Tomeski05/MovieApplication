using System.ComponentModel.DataAnnotations;

namespace MovieApp.Domain
{
    public class ForgotPasswordModel
    {
        [Required, EmailAddress, Display(Name = "Registered email address")]
        public string Email { get; set; }
    }
}
