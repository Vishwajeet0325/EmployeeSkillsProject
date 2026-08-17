using EmployeeSkillsManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSkillsManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeSkill>()
                .HasKey(es => new
                {
                    es.EmployeeId,es.SkillId
                });

            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeSkills).HasForeignKey(es => es.EmployeeId);

            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Skill)
                .WithMany(s => s.EmployeeSkills).HasForeignKey(es => es.SkillId);

            modelBuilder.Entity<Skill>().HasData(

                new Skill
                {
                    Id = 1,
                    Name = "C#"
                },
                new Skill
                {
                    Id = 2,
                    Name = "ASP.NET"
                },
                new Skill
                {
                    Id = 3,
                    Name = "C++"
                },
                new Skill
                {
                    Id = 4,
                    Name = "Java"
                },
                new Skill
                {
                    Id = 5,
                    Name = "JavaScript"
                },
                new Skill
                {
                    Id = 6,
                    Name = "SQL"
                }
            );
        }
    }
}