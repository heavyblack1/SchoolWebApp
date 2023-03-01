using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Data;
using SchoolWebApp.Models;

namespace SchoolWebApp.Services {
    public class SubjectService {
        private ApplicationDbContext _dbContext;

        public SubjectService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Subject>> GetAllAsync() {
            return await _dbContext.Subjects.ToListAsync();
        }

        public async Task<Subject> GetByIdAsync(int id) {
            return await _dbContext.Subjects.FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task CreateAsync(Subject newSubject) {
            await _dbContext.Subjects.AddAsync(newSubject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Subject updatedSubject) {
            // Allow editing of same subject by multiple users
            var existingSubject = await GetByIdAsync(updatedSubject.Id);

            if (existingSubject != null) {
                existingSubject.Name = updatedSubject.Name;

                await _dbContext.SaveChangesAsync();

                // Detach the entity from the context to allow subsequent updates
                _dbContext.Entry(existingSubject).State = EntityState.Detached;
            }
            else {
                throw new InvalidOperationException("Data was deled by someone else in meantime.");
            }
        }

        public async Task DeleteAsync(int id) {
            var subjectToDelete = await _dbContext.Subjects.FirstOrDefaultAsync(st => st.Id == id);
            if (subjectToDelete != null) {
                _dbContext.Subjects.Remove(subjectToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
