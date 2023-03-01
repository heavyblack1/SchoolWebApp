using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebApp.Models {
    [Table("Subjects")]
    public class Subject {
        [Key]
        [ConcurrencyCheck]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
