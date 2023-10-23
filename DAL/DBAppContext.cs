using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QuizApp.Models;

namespace QuizApp.DAL
{
    public class DBAppContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }

        public DBAppContext(DbContextOptions<DBAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>()
               .HasMany(quiz => quiz.Questions)
               .WithOne(question => question.Quiz)
               .HasForeignKey(question => question.QuizId)
               .HasPrincipalKey(quiz => quiz.QuizId)
               .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
