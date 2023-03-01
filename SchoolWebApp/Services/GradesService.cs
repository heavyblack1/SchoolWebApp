using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Data;
using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;

namespace SchoolWebApp.Services {
    public class GradesService {
        private ApplicationDbContext _dbContext;
        public GradesService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<GradesDropdownsViewModel> GetGradesDropdownsValues() {
            var gradesDropdownsViewModel = new GradesDropdownsViewModel() {
                Students = await _dbContext.Students.OrderBy(st => st.LastName).ToListAsync(),
                Subjects = await _dbContext.Subjects.OrderBy(subj => subj.Name).ToListAsync(),
            };
            return gradesDropdownsViewModel;
        }

        internal async Task CreateAsync(GradesVM newGrade) {
            Grade gradeToInsert = new Grade() {
                Student = _dbContext.Students.FirstOrDefault(s => s.Id == newGrade.StudentId),
                Subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == newGrade.SubjectId),
                What = newGrade.What,
                Mark = newGrade.Mark,
                Date = DateTime.Today,
            };
            if (gradeToInsert.Student != null && gradeToInsert.Subject != null) {
                await _dbContext.Grades.AddAsync(gradeToInsert);
                await _dbContext.SaveChangesAsync();
            }
        }

        internal async Task<IEnumerable<Grade>> GetAllAsync() {
            return await _dbContext.Grades.Include(s => s.Student).Include(s => s.Subject).ToListAsync();
        }

        internal async Task<Grade> GetByIdAsync(int id) {
            return await _dbContext.Grades.Include(s => s.Student).Include(s => s.Subject).FirstOrDefaultAsync(gr => gr.Id == id);
        }

        internal async Task UpdateAsync(int id, GradesVM updatedGrade) {
            var gradeToUpdate = await _dbContext.Grades.FirstOrDefaultAsync(gr => gr.Id == updatedGrade.Id);
            if (gradeToUpdate != null) {
                gradeToUpdate.Student = _dbContext.Students.FirstOrDefault(s => s.Id == updatedGrade.StudentId);
                gradeToUpdate.Subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == updatedGrade.SubjectId);
                gradeToUpdate.What = updatedGrade.What;
                gradeToUpdate.Mark = updatedGrade.Mark;
                gradeToUpdate.Date = updatedGrade.Date;

                await _dbContext.SaveChangesAsync();

                // Detach the entity from the context to allow subsequent updates
                _dbContext.Entry(gradeToUpdate).State = EntityState.Detached;
            }
            else {
                throw new InvalidOperationException("Data was deled by someone else in meantime.");
            }
        }
        internal async Task DeleteAsync(int id) {
            var gradeToDelete = await GetByIdAsync(id);
            if (gradeToDelete != null) {
                _dbContext.Grades.Remove(gradeToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
