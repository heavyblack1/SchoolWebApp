using SchoolWebApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebApp.ViewModels {
    public class GradesVM {
        public int Id { get; set; }
        // [DisplayName] attribute is used to specify a display name for the property in the UI, which can be useful for providing more user-friendly labels for fields.
        [DisplayName("Student's Name")]
        public int StudentId { get; set; }
        [DisplayName("Subject")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [StringLength(50, ErrorMessage = "The description must be between 1 and 50 characters", MinimumLength = 1)]
        public string What { get; set; }

        [Required(ErrorMessage = "Please enter a mark")]
        [Range(0, 100, ErrorMessage = "The mark must be between 0 and 100")]
        public int Mark { get; set; }
        public DateTime Date { get; set; }
    }
}
