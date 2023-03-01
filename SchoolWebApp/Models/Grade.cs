using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebApp.Models {

    [Table("Grades")]
    public class Grade {
        [Key] // if properly named [Key] annotation is not needed
        [ConcurrencyCheck]
        public int Id { get; set; }
        [Required]
        public Student Student { get; set; }
        [Required]
        public Subject Subject { get; set; }
        [Required]
        public string What { get; set; }
        [Required]
        public int Mark { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}
