using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebApp.Models {
    [Table("Students")]
    public class Student {
        [Key]
        [ConcurrencyCheck]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a first name.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The first name must be between 2 and 50 characters long.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The last name must be between 2 and 50 characters long.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a valid date of birth.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
    }
}
