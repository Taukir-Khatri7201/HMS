using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Enter registered email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
