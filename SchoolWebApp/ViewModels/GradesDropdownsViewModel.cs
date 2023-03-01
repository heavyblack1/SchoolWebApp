using SchoolWebApp.Models;

/*
Provide the view with the data it needs to populate the dropdown lists for selecting a student and a subject
*/

namespace SchoolWebApp.ViewModels {
    public class GradesDropdownsViewModel {
        public List<Student> Students { get; set; }
        public List<Subject> Subjects { get; set; }
        public GradesDropdownsViewModel() {
            Students = new List<Student>();
            Subjects = new List<Subject>();
        }

    }
}
