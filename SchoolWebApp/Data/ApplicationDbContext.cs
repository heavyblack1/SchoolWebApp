using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Models;

namespace SchoolWebApp.Data {
    public class ApplicationDbContext:DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }

    }
}
