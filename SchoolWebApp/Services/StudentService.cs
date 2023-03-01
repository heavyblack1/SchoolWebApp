using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Data;
using SchoolWebApp.Models;

namespace SchoolWebApp.Services {
    public class StudentService {
        private ApplicationDbContext _dbContext;

        public StudentService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Student>> GetAllAsync() {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id) {
            return await _dbContext.Students.FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task CreateAsync(Student newStudent) {
            await _dbContext.Students.AddAsync(newStudent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Student updatedStudent) {

            // Allow editing of same student by multiple users
            var existingStudent = await GetByIdAsync(updatedStudent.Id);

            if (existingStudent != null) {
                existingStudent.FirstName = updatedStudent.FirstName;
                existingStudent.LastName = updatedStudent.LastName;
                existingStudent.DateOfBirth = updatedStudent.DateOfBirth;

                await _dbContext.SaveChangesAsync();

                // Detach the entity from the context to allow subsequent updates
                _dbContext.Entry(existingStudent).State = EntityState.Detached;
            }
            else {
                throw new InvalidOperationException("Data was deled by someone else in meantime.");
            }

            // can handle only one user
            // if (await GetByIdAsync(updatedStudent.Id) != null) {
            //     _dbContext.Students.Update(updatedStudent);
            //     await _dbContext.SaveChangesAsync();
            // }
            // else {
            //     throw new InvalidOperationException("Data was deled by someone else in meantime.");
            // }
        }

        public async Task DeleteAsync(int id) {
            var studentToDelete = await GetByIdAsync(id);
            if (studentToDelete != null) {
                _dbContext.Students.Remove(studentToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
