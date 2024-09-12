using Microsoft.EntityFrameworkCore;
using StudentGradesApp.Models;

namespace StudentGradesApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeType> GradeTypes { get; set; }
        public DbSet<FinalResult> FinalResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.GradeType)
                .WithMany()
                .HasForeignKey(g => g.GradeTypeId);

            modelBuilder.Entity<FinalResult>()
                .HasOne(fr => fr.Student)
                .WithMany()
                .HasForeignKey(fr => fr.StudentId);
        }

    }
}
