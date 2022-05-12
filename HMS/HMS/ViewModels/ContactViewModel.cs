using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter  your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your feedback")]
        public string Message { get; set; }
    }
}
