using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.DAL;
using System;
using System.Linq;

namespace QuizApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DBAppContext(
                serviceProvider.GetRequiredService<DbContextOptions<DBAppContext>>()))
            {
                context.Database.EnsureCreated();

                // Look for any questions
                if (context.Questions.Any())
                {
                    return; // DB has been seeded
                }


                // Create a quiz
                var quiz = context.Quizzes.Add(
                    new Quiz
                    {
                        Name = "PCC 50th Year Gala Quiz"
                    }
                );
                context.SaveChanges();
               
                context.Questions.AddRange(
                     new Question
                     {
                         QuizId = quiz.Entity.QuizId,
                         Text = "The best performance at the PCC 50th Gala was The Planets: Jupiter?",
                         Answer = true
                     }
                );
                context.SaveChanges();
            }
        }
    }
}